using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using INSS.EIIR.Models.ExtractModels;
using Azure.Storage.Blobs;
using System.Xml.Serialization;
using System.Text;
using AutoMapper;
using Azure.Search.Documents.Models;
using Azure.Search.Documents;
using Azure;
using Azure.Search.Documents.Indexes;
using System.IO.Compression;

namespace INSS.EIIR.Functions
{
    public static class EiirDailyExtract 
    {
        [FunctionName("extracts")]
        public static async Task<IActionResult> Run(
             [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
             ILogger log , Microsoft.Azure.WebJobs.ExecutionContext context)
        {
            SearchClient SearchClient = EiirDailyExtractHelpers.GetSearchClient(context);

            var results = RunQueries(SearchClient);
            var extractReport = EiirDailyExtractSerializer.SerializeToXml(results);
            await EiirUpload.UploadBlob(extractReport);

            string responseMessage = "Extract Triggered Successfully!";
            log.LogInformation(responseMessage);
            return new OkObjectResult(responseMessage);
        }

        

        private static SearchResults<IndividualSearchResult> RunQueries(SearchClient srchclient)
        {
            SearchOptions options;
            options = new SearchOptions()
            {
                IncludeTotalCount = true,
                Filter = "",
                OrderBy = { "" }
            };

            return srchclient.Search<IndividualSearchResult>("*", options);

        }
    }
}

