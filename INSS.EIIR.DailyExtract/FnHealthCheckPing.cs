using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace INSS.EIIR.DailyExtract
{
    public class FnHealthCheckPing
    {

        private readonly ILogger<FnHealthCheckPing> _logger;

        public FnHealthCheckPing(ILogger<FnHealthCheckPing> logger)
        {
            _logger = logger;
        }

        [Function("FnHealthCheckPing")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Health/Ping")] HttpRequestData req)
        {
            return new OkResult();
        }
    }
}
