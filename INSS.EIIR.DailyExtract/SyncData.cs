
using INSS.EIIR.DataSync.Application;
using INSS.EIIR.DataSync.Application.UseCase.SyncData.Model;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Functions.Worker;
using System;
using INSS.EIIR.Models.SyncData;

namespace INSS.EIIR.DailyExtract
{
    public class SyncData
    {
        private readonly ILogger<SyncData> _logger;
        private readonly IRequestResponseUseCase<SyncDataRequest, SyncDataResponse> _responseUseCase;


        public SyncData(ILogger<SyncData> logger, IRequestResponseUseCase<SyncDataRequest, SyncDataResponse> responseUseCase)
        {
            _logger = logger;
            _responseUseCase = responseUseCase;

        }

        [Function(nameof(SyncData))]
        public async Task Run([ActivityTrigger] SyncDataRequest inputs )
        {
            _logger.LogInformation("SyncData started");

            //inputs to be passed to Handle() method... once implemented
            var response = await _responseUseCase.Handle(inputs);

            if (response.IsError)
            {
                _logger.LogWarning($"SyncData failed with {response.ErrorCount} errors. Message: {response.ErrorMessage}");
            }
            else
            {
                _logger.LogInformation($"SyncData completed successfully at: {DateTime.Now}");
            }
        }
    }
}
