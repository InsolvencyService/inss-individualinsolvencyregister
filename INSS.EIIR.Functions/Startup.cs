using AutoMapper;
using Azure;
using Azure.Messaging.ServiceBus;
using Azure.Search.Documents.Indexes;
using INSS.EIIR.AzureSearch.Services;
using INSS.EIIR.AzureSearch.Services.ODataFilters;
using INSS.EIIR.AzureSearch.Services.QueryServices;
using INSS.EIIR.Data.AutoMapperProfiles;
using INSS.EIIR.Data.Models;
using INSS.EIIR.DataAccess;
using INSS.EIIR.Functions;
using INSS.EIIR.Interfaces.AzureSearch;
using INSS.EIIR.Interfaces.DataAccess;
using INSS.EIIR.Interfaces.Messaging;
using INSS.EIIR.Interfaces.Services;
using INSS.EIIR.Models.AutoMapperProfiles;
using INSS.EIIR.Models.Configuration;
using INSS.EIIR.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;

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
                mc.AddProfile(new ExtractMapper());
                mc.AddProfile(new SubscriberMapper());
            });

            var mapper = mapperConfig.CreateMapper();
            builder.Services.AddSingleton(mapper);

            var connectionString = Environment.GetEnvironmentVariable("database__connectionstring");
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException("database__connectionstring missing");

            var serviceBusPubConnectionString = Environment.GetEnvironmentVariable("servicebus__publisherconnectionstring");
            if (string.IsNullOrEmpty(serviceBusPubConnectionString))
                throw new ArgumentNullException("servicebus__publisherconnectionstring is missing");

            builder.Services.AddTransient(_ =>
            {
                return new EIIRContext(connectionString);
            });

            builder.Services.AddTransient(_ =>
            {
                var searchServiceUrl = Environment.GetEnvironmentVariable("EIIRIndexUrl");
                var adminApiKey = Environment.GetEnvironmentVariable("EIIRApiKey");

                return CreateSearchServiceClient(searchServiceUrl, adminApiKey);
            });

            builder.Services.AddDbContext<EIIRExtractContext>(options =>
                options.UseSqlServer(connectionString));

            var blobconnectionstring = Environment.GetEnvironmentVariable("blobconnectionstring");
            if (string.IsNullOrEmpty(blobconnectionstring))
                throw new ArgumentNullException("blobconnectionstring is missing");

            builder.Services.AddOptions<DatabaseConfig>()
               .Configure<IConfiguration>((settings, configuration) =>
               {
                   configuration.GetSection("database").Bind(settings);
               });
            builder.Services.AddOptions<ServiceBusConfig>()
               .Configure<IConfiguration>((settings, configuration) =>
               {
                   configuration.GetSection("servicebus").Bind(settings);
               });
            builder.Services.AddOptions<NotifyConfig>()
                .Configure<IConfiguration>((settings, configuration) =>
                {
                    configuration.GetSection("notify").Bind(settings);
                });

            builder.Services.AddAzureClients(clientsBuilder =>
            {
                clientsBuilder.AddServiceBusClient(serviceBusPubConnectionString)
                  .WithName("ServiceBusPublisher")
                  .ConfigureOptions(options =>
                  {
                      options.TransportType = ServiceBusTransportType.AmqpWebSockets;
                  });

                clientsBuilder.AddBlobServiceClient(blobconnectionstring);
            });

            builder.Services.AddScoped<IServiceBusMessageSender, ServiceBusMessageSender>();
            builder.Services.AddScoped<IExtractRepository, ExtractRepository>();
            builder.Services.AddScoped<ISubscriberRepository, SubscriberRepository>();
            builder.Services.AddScoped<IExtractDataProvider, ExtractDataProvider>();
            builder.Services.AddScoped<ISubscriberDataProvider, SubscriberDataProvider>();
            builder.Services.AddScoped<INotificationService, NotificationService>();

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
