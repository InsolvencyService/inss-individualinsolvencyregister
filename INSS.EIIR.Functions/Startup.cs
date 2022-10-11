using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using AutoMapper;
using Azure;
using Azure.Search.Documents.Indexes;
using INSS.EIIR.AzureSearch.Services;
using INSS.EIIR.AzureSearch.Services.ODataFilters;
using INSS.EIIR.AzureSearch.Services.QueryServices;
using INSS.EIIR.Data.Models;
using INSS.EIIR.DataAccess;
using INSS.EIIR.Functions;
using INSS.EIIR.Interfaces.AzureSearch;
using INSS.EIIR.Interfaces.DataAccess;
using INSS.EIIR.Interfaces.Services;
using INSS.EIIR.Models.AutoMapperProfiles;
using INSS.EIIR.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;

[assembly: FunctionsStartup(typeof(Startup))]

namespace INSS.EIIR.Functions
{
    [ExcludeFromCodeCoverage]
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            //builder.Services.AddApplicationInsightsTelemetry();

            builder.Services.AddHttpClient();
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // Auto Mapper Configurations
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new IndividualSearchMapper());
            });

            var mapper = mapperConfig.CreateMapper();
            builder.Services.AddSingleton(mapper);

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

            builder.Services.AddTransient<IIndexService, IndividualSearchIndexService>();
            builder.Services.AddTransient<IIndividualRepository, IndividualRepository>();
            builder.Services.AddTransient<IIndividualQueryService, IndividualQueryService>();
            builder.Services.AddTransient<ISearchDataProvider, SearchDataProvider>();

            builder.Services.AddTransient<IIndiviualSearchFilter, IndividualSearchCourtFilter>();
            builder.Services.AddTransient<IIndiviualSearchFilter, IndividualSearchCourtNameFilter>();

            builder.Services.AddTransient<ISearchTermFormattingService, SearchTermFormattingService>();
            builder.Services.AddTransient<ISearchCleaningService, SearchCleaningService>();
        }

        private static SearchIndexClient CreateSearchServiceClient(string searchServiceUrl, string adminApiKey)
        {
            var serviceClient = new SearchIndexClient(new Uri(searchServiceUrl), new AzureKeyCredential(adminApiKey));

            return serviceClient;
        }

        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            var jitter = new Random();

            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                .WaitAndRetryAsync(3, retryAttempt =>
                    TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                    + TimeSpan.FromMilliseconds(jitter.Next(0, 100)));
        }
    }
}
