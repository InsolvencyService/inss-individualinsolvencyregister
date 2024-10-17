
using INSS.EIIR.DataSync.Application;
using INSS.EIIR.DataSync.Application.UseCase.SyncData.Model;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.Functions.Worker;
using System;
using INSS.EIIR.Interfaces.DataAccess;
using System.Drawing;

namespace INSS.EIIR.DailyExtract
{
    public class SyncData
    {
        private readonly ILogger<SyncData> _logger;
        private readonly IResponseUseCase<SyncDataResponse> _responseUseCase;


        public SyncData(ILogger<SyncData> logger, IResponseUseCase<SyncDataResponse> responseUseCase)
        {
            _logger = logger;
            _responseUseCase = responseUseCase;

        }

        [Function("SyncData")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("SyncData started");

            var response = await _responseUseCase.Handle();

            if (response.IsError)
            {
                return new BadRequestObjectResult($"SyncData failed with {response.ErrorCount} errors. Message: {response.ErrorMessage}");
            }
            else
            {
                return new OkResult();
            }
        }
    }
}
