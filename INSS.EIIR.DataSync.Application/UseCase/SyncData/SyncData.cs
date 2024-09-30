using INSS.EIIR.DataSync.Application.UseCase.SyncData.Infrastructure;
using INSS.EIIR.DataSync.Application.UseCase.SyncData.Model;
using INSS.EIIR.DataSync.Application.UseCase.SyncData.Service;
using Microsoft.Extensions.Logging;
using INSS.EIIR.Models.CaseModels;
using System.Reflection;

namespace INSS.EIIR.DataSync.Application.UseCase.SyncData
{
    public class SyncData : IResponseUseCase<SyncDataResponse>
    {
        private readonly SyncDataOptions _options;
        private readonly TransformService _transformService;
        private readonly ValidationService _validation;
        private readonly ILogger<SyncData> _logger;

        public SyncData(SyncDataOptions options, ILogger<SyncData> logger)
        {
            _options = options;
            _logger = logger;
            _transformService = new TransformService(_options.TransformRules);
            _validation = new ValidationService();
        }

        public async Task<SyncDataResponse> Handle()
        {
            int numErrors = 0;

            foreach (IDataSink<InsolventIndividualRegisterModel> sink in _options.DataSinks)
            {
                await sink.Start();
            }

            //Following line is commented out as FailureSink not fully implemented and will cause crash if deployed
            //await _options.FailureSink.Start();

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
                            break; // skip to the next item.
                        }

                        // transform
                        var transformResponse = await _transformService.Transform(model);

                        if (transformResponse.IsError)
                        {
                            await SinkFailure(model.Id, transformResponse);                                                    
                            numErrors++;
                        }
                        else
                        {
                            foreach (IDataSink<InsolventIndividualRegisterModel> sink in _options.DataSinks)
                            {
                                var sinkResponse = await sink.Sink(transformResponse.Model);
                                if (sinkResponse.IsError)
                                {
                                    _logger.LogError($"Error sinking {model.Id} to {sink.GetType()}");
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
                await sink.Complete();
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