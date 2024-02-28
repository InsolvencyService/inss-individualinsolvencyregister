using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace INSS.EIIR.Functions
{
    public class FnHealthCheckFull
    {
        private readonly HealthCheckService _healthCheckService;

        public FnHealthCheckFull(HealthCheckService healthCheckService)
        {
            _healthCheckService = healthCheckService;
        }

        [FunctionName("FnHealthCheckFull")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Health")] HttpRequest req,
            ILogger log)
        {
            var result = await _healthCheckService.CheckHealthAsync();
            return new OkObjectResult(result);
        }
    }
}
