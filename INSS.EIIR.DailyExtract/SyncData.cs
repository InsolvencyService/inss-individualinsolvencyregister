
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
        private readonly IExtractRepository _eiirRepository;

        public SyncData(ILogger<SyncData> logger, IResponseUseCase<SyncDataResponse> responseUseCase, IExtractRepository extractRepository)
        {
            _logger = logger;
            _responseUseCase = responseUseCase;
            _eiirRepository = extractRepository;
        }

        [Function("SyncData")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("SyncData started");


            #region Pre-Conditions check
            var extractJob = _eiirRepository.GetExtractAvailable();

            var today = DateOnly.FromDateTime(DateTime.Now);

            var extractjobError = $"ExtractJob not found for today [{today}], IIR snapshot has not run";
            var snapshotError = $"IIR Snapshot has not yet run today [{today}]";
            var extractAlreadyExistsError = $"Subscriber xml / zip file creation has already ran successfully on [{today}]";

            if (extractJob == null)
            {
                return new BadRequestObjectResult(extractjobError);
            }

            if (extractJob.SnapshotCompleted?.ToLowerInvariant() == "n")
            {
                return new BadRequestObjectResult(snapshotError);
            }

            if (extractJob.ExtractCompleted?.ToLowerInvariant() == "y")
            {
                return new BadRequestObjectResult(extractAlreadyExistsError);
            }
            #endregion Pre-Conditions check

            var response = await _responseUseCase.Handle();

            if (response.IsError)
            {
                return new BadRequestObjectResult($"SyncData failed with {response.ErrorCount} erors");
            }
            else
            {
                return new OkResult();
            }
        }
    }
}
