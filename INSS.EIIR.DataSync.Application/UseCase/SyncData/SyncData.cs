using INSS.EIIR.DataSync.Application.UseCase.SyncData.Infrastructure;
using INSS.EIIR.DataSync.Application.UseCase.SyncData.Exceptions;
using INSS.EIIR.DataSync.Application.UseCase.SyncData.Model;
using INSS.EIIR.DataSync.Application.UseCase.SyncData.Service;
using Microsoft.Extensions.Logging;
using INSS.EIIR.Interfaces.DataAccess;
using INSS.EIIR.Models.SyncData;

namespace INSS.EIIR.DataSync.Application.UseCase.SyncData
{
    public class SyncData : IRequestResponseUseCase<SyncDataRequest, SyncDataResponse>
    {
        private readonly SyncDataOptions _options;
        private readonly TransformService _transformService;
        private readonly ValidationService _validation;
        private readonly ILogger<SyncData> _logger;
        private bool _swapIndexAndZipXml = true;
        private bool _testModeNotActive = true;
        private readonly IExtractRepository _eiirRepository;

        public SyncData(SyncDataOptions options, IExtractRepository extractRepository, ILogger<SyncData> logger)
        {
            _options = options;
            _logger = logger;
            _transformService = new TransformService(_options.TransformRules);
            _validation = new ValidationService(_options.ValidationRules);
            _eiirRepository = extractRepository;
        }

        public async Task<SyncDataResponse> Handle(SyncDataRequest request)
        {
            int numErrors = 0;

            await _options.FailureSink.Start();

            SyncDataResponse response = CheckPreconditions();

            #region Pre-Conditions check
            if (IgnorePreConditionChecks(request))
            {
                _logger.LogWarning($"SyncData pre-condition checks ignored {(response.ErrorMessage != string.Empty ? "Error: " + response.ErrorMessage : "")}");
            }
            else
            {
                if (response.ErrorMessage != "")
                {
                    response.ErrorCount++;
                    await SinkFailure("SyncData Initialisation Failure", response);
                    return response;
                }
            }
            #endregion Pre-Conditions check

            //Check if no datasources have been specified 
            if (AreNoDataSourceSpecified(request))
            {
                response = new SyncDataResponse() { ErrorCount = 1, ErrorMessage = "No DataSources detected when calling SyncData" };
                await SinkFailure("SyncData Initialisation Failure", response);
                return response;
            }

            //Check if no sinks are enabled
            if (AreNoSinksEnabled(request))
            {
                response = new SyncDataResponse() { ErrorCount = 1, ErrorMessage = "All data sinks disabled detected when calling SyncData" };
                await SinkFailure("SyncData Initialisation Failure", response);
                return response;
            }

            if (IsTestModeActive(request))
            {
                _testModeNotActive = false;
                _logger.LogWarning("Testmode is active Zip file will not be created for XML Extract nor Search Index swapped");
            }

            foreach (IDataSink<InsolventIndividualRegisterModel> sink in GetEnabledDataSinks(request))
            {
                _logger.LogWarning(sink.Description);
                await sink.Start();
            }

            foreach (IDataSourceAsync<InsolventIndividualRegisterModel> source in GetSpecifiedDataSources(request))
            {
                try
                {
                    _logger.LogWarning($"Processing DataSource for: {source.Description}");
                    await foreach (var model in source.GetInsolventIndividualRegistrationsAsync())
                    {
                        // validate
                        var validationResponse = await ValidateModel(model);

                        // sink failure
                        if (!validationResponse.IsValid)
                        {
                            await SinkFailure(model.Id, validationResponse);
                            numErrors++;
                            _swapIndexAndZipXml = false;
                            break; // skip to the next item.
                        }

                        // transform
                        var transformResponse = await TransformModel(model);

                        if (transformResponse.IsError)
                        {
                            await SinkFailure(model.Id, transformResponse);
                            numErrors++;
                            _swapIndexAndZipXml = false;
                        }
                        else
                        {
                            foreach (IDataSink<InsolventIndividualRegisterModel> sink in GetEnabledDataSinks(request))
                            {
                                var sinkResponse = await SinkModel(transformResponse.Model, sink);
                                if (sinkResponse.IsError)
                                {
                                    await SinkFailure(model.Id, sinkResponse);
                                    _swapIndexAndZipXml = false;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _swapIndexAndZipXml = false;
                    throw;
                }
            }

            foreach (IDataSink<InsolventIndividualRegisterModel> sink in GetEnabledDataSinks(request))
            {
                await sink.Complete(_swapIndexAndZipXml && _testModeNotActive);
            }

            await _options.FailureSink.Complete();

            if (numErrors > 0)
            {
                _logger.LogError($"There were {numErrors} when processing the extract for {DateTime.Now.ToString()}");
            }

            return new SyncDataResponse()
            {
                ErrorCount = numErrors
            };
        }

        private bool IsTestModeActive(SyncDataRequest request)
        {
            return (request.Modes & SyncDataEnums.Mode.Test) == SyncDataEnums.Mode.Test;
        }

        private IEnumerable<IDataSourceAsync<InsolventIndividualRegisterModel>> GetSpecifiedDataSources(SyncDataRequest request)
        {
            return _options.DataSources.Where(s => (s.Type & request.DataSources) == s.Type);
        }

        private IEnumerable<IDataSink<InsolventIndividualRegisterModel>> GetEnabledDataSinks(SyncDataRequest request)
        {
            return _options.DataSinks.Where(s => (s.EnabledCheckBit & ~request.Modes) == s.EnabledCheckBit);
        }

        private static bool AreNoSinksEnabled(SyncDataRequest request)
        {
            return (request.Modes & (SyncDataEnums.Mode.DisableXMLExtract | SyncDataEnums.Mode.DisableIndexRebuild))
                                == (SyncDataEnums.Mode.DisableXMLExtract | SyncDataEnums.Mode.DisableIndexRebuild);
        }

        private bool AreNoDataSourceSpecified(SyncDataRequest request)
        {
            return request.DataSources == SyncDataEnums.Datasource.None || _options.DataSources.Where(s => (s.Type & request.DataSources) == s.Type).Count() == 0;
        }

        private static bool IgnorePreConditionChecks(SyncDataRequest request)
        {
            return (request.Modes & SyncDataEnums.Mode.IgnorePreConditionChecks) == SyncDataEnums.Mode.IgnorePreConditionChecks;
        }

        private async Task<DataSinkResponse> SinkModel(InsolventIndividualRegisterModel model, IDataSink<InsolventIndividualRegisterModel> sink)
        {
            try
            {
                return await sink.Sink(model);
            }
            //For when DataSink logic throws an unexpected exception, not when it gracefully finds errors
            catch (Exception ex) 
            { 
                throw new DataSinkException($"Error sinking model for {model.Id} to sink {sink.GetType()}", ex);
            }
        }

        private async Task<TransformResponse> TransformModel(InsolventIndividualRegisterModel model)
        {
            try
            {
                return await _transformService.Transform(model);
            }
            //For when Transform logic throws an unexpected exception, not when it gracefully finds errors
            catch (Exception ex)
            {
                throw new TransformRuleException($"Exception transforming {model.Id}", ex);
            }       
        }

        private async Task<ValidationResponse> ValidateModel(InsolventIndividualRegisterModel model)
        {
            try
            {
                return await _validation.Validate(model);
            }
            //For when Validation logic throws an unexpected exception, not when it gracefully finds errors
            catch (Exception ex)
            {
                throw new ValidationRuleException($"Exception validating {model.Id}", ex);
            }
        }

        private async Task SinkFailure(string id, SyncFailure failure)
        {
            try
            {
                await _options.FailureSink.Sink(failure);
            }
            catch (Exception ex)
            {
                throw new DataSinkException($"Error sinking failure for {id} to sink {_options.FailureSink.GetType()}", ex);
            }
        }


        private SyncDataResponse CheckPreconditions()
        {
            var response = new SyncDataResponse() { ErrorCount = 0, ErrorMessage = string.Empty };

            var extractJob = _eiirRepository.GetExtractAvailable();

            var today = DateOnly.FromDateTime(DateTime.Now);

            var extractjobError = $"ExtractJob not found for today [{today}], IIR snapshot has not run";
            var snapshotError = $"IIR Snapshot has not yet run today [{today}]";
            var extractAlreadyExistsError = $"Subscriber xml / zip file creation has already ran successfully on [{today}]";

            if (extractJob == null)
            {
                response.ErrorMessage = extractjobError;
                return response;
            }

            if (extractJob.SnapshotCompleted?.ToLowerInvariant() == "n")
            {
                response.ErrorMessage = snapshotError;
                return response;
            }

            if (extractJob.ExtractCompleted?.ToLowerInvariant() == "y")
            { 
                response.ErrorMessage = extractAlreadyExistsError;
                return response;
            }

            return response;
        }

    }
}