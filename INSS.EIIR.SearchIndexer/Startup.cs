using System;
using System.Diagnostics.CodeAnalysis;
using Azure;
using Azure.Search.Documents.Indexes;
using INSS.EIIR.Data.Models;
using INSS.EIIR.DataAccess;
using INSS.EIIR.Interfaces.DataAccess;
using INSS.EIIR.Interfaces.SearchIndexer;
using INSS.EIIR.SearchIndexer.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(INSS.EIIR.SearchIndexer.Startup))]

namespace INSS.EIIR.SearchIndexer
{
    [ExcludeFromCodeCoverage]
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            //builder.Services.AddApplicationInsightsTelemetry();

            builder.Services.AddHttpClient();
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            builder.Services.AddTransient(_ =>
            {
                var connectionString = Environment.GetEnvironmentVariable("iirwebdbContextConnectionString");
                return new EIIRContext(connectionString);
            });

            builder.Services.AddTransient(_ =>
            {
                var searchServiceUrl = Environment.GetEnvironmentVariable("EIIRIndexUrl");
                var adminApiKey = Environment.GetEnvironmentVariable("EIIRApiKey");

                return CreateSearchServiceClient(searchServiceUrl, adminApiKey);
            });

            builder.Services.AddTransient<IIndexService, IndexService>();
            builder.Services.AddTransient<IIndividualRepository, IndividualRepository>();
            builder.Services.AddTransient<ISearchDataProvider, SearchDataProvider>();
        }

        private static SearchIndexClient CreateSearchServiceClient(string searchServiceUrl, string adminApiKey)
        {
            var serviceClient = new SearchIndexClient(new Uri(searchServiceUrl), new AzureKeyCredential(adminApiKey));
            return serviceClient;
        }
    }
}
