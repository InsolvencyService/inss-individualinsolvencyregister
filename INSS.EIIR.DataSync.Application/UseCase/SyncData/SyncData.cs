using INSS.EIIR.DataSync.Application.UseCase.SyncData.Infrastructure;
using INSS.EIIR.DataSync.Application.UseCase.SyncData.Model;
using INSS.EIIR.DataSync.Application.UseCase.SyncData.Service;
using Microsoft.Extensions.Logging;
using INSS.EIIR.Models.CaseModels;
using System.Reflection;
using INSS.EIIR.Interfaces.DataAccess;

namespace INSS.EIIR.DataSync.Application.UseCase.SyncData
{
    public class SyncData : IResponseUseCase<SyncDataResponse>
    {
        private readonly SyncDataOptions _options;
        private readonly TransformService _transformService;
        private readonly ValidationService _validation;
        private readonly ILogger<SyncData> _logger;
        private bool _swapIndexAndZipXml = true;
        private readonly IExtractRepository _eiirRepository;

        public SyncData(SyncDataOptions options, IExtractRepository extractRepository, ILogger<SyncData> logger)
        {
            _options = options;
            _logger = logger;
            _transformService = new TransformService(_options.TransformRules);
            _validation = new ValidationService();
            _eiirRepository = extractRepository;
        }

        public async Task<SyncDataResponse> Handle()
        {
            var resp = new SyncDataResponse() { ErrorCount = 0, ErrorMessage = string.Empty };

            int numErrors = 0;

            await _options.FailureSink.Start();

            #region Pre-Conditions check
            var extractJob = _eiirRepository.GetExtractAvailable();

            var today = DateOnly.FromDateTime(DateTime.Now);

            var extractjobError = $"ExtractJob not found for today [{today}], IIR snapshot has not run";
            var snapshotError = $"IIR Snapshot has not yet run today [{today}]";
            var extractAlreadyExistsError = $"Subscriber xml / zip file creation has already ran successfully on [{today}]";

            if (extractJob == null)
                resp.ErrorMessage = extractjobError;

            if (extractJob.SnapshotCompleted?.ToLowerInvariant() == "n")
                resp.ErrorMessage = snapshotError;

            if (extractJob.ExtractCompleted?.ToLowerInvariant() == "y")
                resp.ErrorMessage = extractAlreadyExistsError;

            if (resp.ErrorMessage != "")
            {
                resp.ErrorCount++;
                await SinkFailure("SyncData Initialisation Failure", resp);
                return resp;
            }
            #endregion Pre-Conditions check


            foreach (IDataSink<InsolventIndividualRegisterModel> sink in _options.DataSinks)
            {
                await sink.Start();
            }

            foreach (IDataSourceAsync<InsolventIndividualRegisterModel> source in  _options.DataSources) 
            {
                try
                {
                    await foreach (var model in source.GetInsolventIndividualRegistrationsAsync())
                    {
                        // validate
                        var validationResponse = await _validation.Validate(model);

                        // sink failure
                        if (!validationResponse.IsValid)
                        {
                            await SinkFailure(model.Id, validationResponse);
                            numErrors++;
                            _swapIndexAndZipXml = false;
                            break; // skip to the next item.
                        }

                        // transform
                        var transformResponse = await _transformService.Transform(model);

                        if (transformResponse.IsError)
                        {
                            await SinkFailure(model.Id, transformResponse);
                            numErrors++;
                            _swapIndexAndZipXml = false;
                        }
                        else
                        {
                            foreach (IDataSink<InsolventIndividualRegisterModel> sink in _options.DataSinks)
                            {
                                var sinkResponse = await sink.Sink(transformResponse.Model);
                                if (sinkResponse.IsError)
                                {
                                    await SinkFailure(model.Id, sinkResponse);
                                    _logger.LogError($"Error sinking {model.Id} to {sink.GetType()}");
                                    _swapIndexAndZipXml= false;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Unhandled error transforming and sinking item");
                }
            }

            foreach (IDataSink<InsolventIndividualRegisterModel> sink in _options.DataSinks)
            {
                await sink.Complete(_swapIndexAndZipXml);
            }

            //Following line is commented out as FailureSink not fully implemented and will cause crash if deployed
            //await _options.FailureSink.Complete();

            if (numErrors > 0)
            {
                _logger.LogError($"There were {numErrors} when processing the extract for {DateTime.Now.ToString()}");
            }

            return new SyncDataResponse()
            {
                ErrorCount = numErrors
            };
        }

        private async Task SinkFailure(string id, SyncFailure failure)
        {
            try
            {
                await _options.FailureSink.Sink(failure);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error sinking {id} to failure sink {_options.FailureSink.GetType()}");
            }
        }
    }
}