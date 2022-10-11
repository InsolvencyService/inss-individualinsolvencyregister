using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace INSS.EIIR.Functions
{
    public static class FnHealthCheckPing
    {
        [FunctionName("FnHealthCheckPing")]
        public static async Task<IActionResult> Run(
           [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Health/Ping")] HttpRequest req,
            ILogger log)
        {
            return new OkResult();
        }
    }
}
