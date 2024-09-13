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

            try
            {
                foreach (IDataSink<InsolventIndividualRegisterModel> sink in _options.DataSinks)
                {
                    await sink.Start();
                }

                //await _options.FailureSink.Start();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unhandled error starting sinks");
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

            try
            {
                foreach (IDataSink<InsolventIndividualRegisterModel> sink in _options.DataSinks) 
                {
                    await sink.Complete();
                }

                //await _options.FailureSink.Complete();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unhandled error completing sinks");
            }

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