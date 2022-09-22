using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using System;

namespace INSS.EIIR.Functions
{
    public static class EiirDailyExtractHelpers
    {

        public static string GetDisclaimerText()
        {
            return "While every effort has been made to ensure that the information provided is accurate, occasionally errors may occur. If you identify information which appears to be incorrect or omitted, please inform The Insolvency Service so that we can investigate the matter and correct the database as required.The Insolvency Case Details are taken from the Court Order made on the Order Date, and include the address(es) from which debts were incurred.They cannot be changed without the consent of the Court. The Individual Details may have changed since the Court Order but, even so, they might not reflect the person's current address or occupation at the time you make your search, and they should not be relied on as such. The Insolvency Service cannot accept responsibility for any errors or ommissions as a result of negligence or otherwise. Please note that The Insolvenmcy Service and Official Receivers cannot provide legal or financial advice. You should seek this from a Citizen's Advice Bureau, a solicitor, a qualified accountant, an authorised Insolvency Practitioner, reputable financial advisor or advice centre. The Individual Insolvency Register is a publicly available register and The Insolvency Service does not endorse, nor make any representations regarding, any use made of the data on the register by third parties.";
        }

        public  static SearchClient GetSearchClient(Microsoft.Azure.WebJobs.ExecutionContext context)
        {
            IConfigurationRoot config = GetConfig(context);

            var indexName = config["indexName"];
            var serviceName = config["serviceName"];
            Uri serviceEndpoint = new Uri($"https://{serviceName}.search.windows.net/");
            AzureKeyCredential credential = new AzureKeyCredential(config["apiKey"]);
            SearchIndexClient adminClient = new SearchIndexClient(serviceEndpoint, credential);


            SearchClient searchClient = new SearchClient(serviceEndpoint, indexName, credential);
            return searchClient;
        }

        private static IConfigurationRoot GetConfig(ExecutionContext context)
        {
            return new ConfigurationBuilder()
                           .SetBasePath(context.FunctionAppDirectory)
                           .AddJsonFile("local.settings.json", true, true)
                           .AddEnvironmentVariables().Build();
        }
    }
}