using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace INSS.EIIR.DailyExtract
{
    public class FnHealthCheckPing
    {
        [FunctionName("FnHealthCheckPing")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Health/Ping")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Health Check Pinged");
            return new OkResult();
        }
    }
}
