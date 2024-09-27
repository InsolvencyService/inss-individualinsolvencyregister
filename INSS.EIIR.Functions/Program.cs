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
using INSS.EIIR.Interfaces.Storage;
using INSS.EIIR.Models.AutoMapperProfiles;
using INSS.EIIR.Models.Configuration;
using INSS.EIIR.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using System;

using Microsoft.Azure.Functions.Worker;

using Microsoft.Extensions.Hosting;
using INSS.EIIR.AzureSearch.IndexMapper;
using INSS.EIIR.StubbedTestData;


var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services => {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();


        services.AddHttpClient();
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        services.AddGetIndexMapper(new IndexMapperOptions()
        {
            TableStorageAccountName = Environment.GetEnvironmentVariable("TableStorageAccountName"),
            TableStorageUri = Environment.GetEnvironmentVariable("TableStorageUri"),
            TableStorageKey = Environment.GetEnvironmentVariable("TableStorageKey")
        });

        services.AddHealthChecks();

        // Auto Mapper Configurations
        var mapperConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new IndividualSearchMapper());
            mc.AddProfile(new ExtractMapper());
            mc.AddProfile(new SubscriberMapper());
            mc.AddProfile(new FeedbackMapper());
        });

        var mapper = mapperConfig.CreateMapper();
        services.AddSingleton(mapper);

        var connectionString = Environment.GetEnvironmentVariable("database__connectionstring");
        if (string.IsNullOrEmpty(connectionString))
            throw new ArgumentNullException("database__connectionstring missing");

        var serviceBusPubConnectionString = Environment.GetEnvironmentVariable("servicebus__publisherconnectionstring");
        if (string.IsNullOrEmpty(serviceBusPubConnectionString))
            throw new ArgumentNullException("servicebus__publisherconnectionstring is missing");

        var notifyConnectionString = Environment.GetEnvironmentVariable("notify__connectionstring");
        if (string.IsNullOrEmpty(notifyConnectionString))
            throw new ArgumentNullException("notify__connectionstring is missing");

        var storageConnectionString = Environment.GetEnvironmentVariable("storageconnectionstring");
        if (string.IsNullOrEmpty(storageConnectionString))
            throw new ArgumentNullException("storageconnectionstring is missing");

        services.AddTransient(_ =>
        {
            return new EIIRContext(connectionString);
        });

        services.AddTransient(_ =>
        {
            var searchServiceUrl = Environment.GetEnvironmentVariable("EIIRIndexUrl");
            var adminApiKey = Environment.GetEnvironmentVariable("EIIRApiKey");

            return new SearchIndexClient(new Uri(searchServiceUrl), new AzureKeyCredential(adminApiKey));
        });

        services.AddDbContext<EIIRExtractContext>(options => options.UseSqlServer(connectionString));

        services.AddOptions<DatabaseConfig>()
           .Configure<IConfiguration>((settings, configuration) =>
           {
               configuration.GetSection("database").Bind(settings);
           });

        services.AddOptions<ServiceBusConfig>()
           .Configure<IConfiguration>((settings, configuration) =>
           {
               configuration.GetSection("servicebus").Bind(settings);
           });

        services.AddOptions<NotifyConfig>()
            .Configure<IConfiguration>((settings, configuration) =>
            {
                configuration.GetSection("notify").Bind(settings);
            });

        services.AddAzureClients(clientsBuilder =>
        {
            clientsBuilder.AddTableServiceClient(storageConnectionString);

            clientsBuilder.AddServiceBusClient(serviceBusPubConnectionString)
              .WithName("ServiceBusPublisher_ExtractJob")
              .ConfigureOptions(options =>
              {
                  options.TransportType = ServiceBusTransportType.AmqpWebSockets;
              });

            clientsBuilder.AddServiceBusClient(notifyConnectionString)
              .WithName("ServiceBusPublisher_Notify")
              .ConfigureOptions(options =>
              {
                  options.TransportType = ServiceBusTransportType.AmqpWebSockets;
              });

            clientsBuilder.AddBlobServiceClient(storageConnectionString);
        });

        services.AddScoped<IServiceBusMessageSender, ServiceBusMessageSender>();
        services.AddScoped<IExtractRepository, ExtractRepository>();
        services.AddScoped<ISubscriberRepository, SubscriberRepository>();
        services.AddScoped<IFeedbackRepository, FeedbackRepository>();

        services.AddScoped<IExtractDataProvider, ExtractDataProvider>();
        services.AddScoped<ISubscriberDataProvider, SubscriberDataProvider>();
        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<IFeedbackDataProvider, FeedbackDataProvider>();

        services.AddTransient<IIndexService, IndividualSearchIndexService>();
        services.AddTransient<IIndividualRepository, IndividualRepository>();

        Boolean useFakeData = false;
        Boolean.TryParse(Environment.GetEnvironmentVariable("UseFakedDataSources"), out useFakeData);
        if (useFakeData)
            services.AddTransient<IIndividualQueryService, IndividualQueryServiceStubbed>();
        else
            services.AddTransient<IIndividualQueryService, IndividualQueryService>();


        services.AddTransient<ISearchDataProvider, SearchDataProvider>();



        services.AddTransient<IIndiviualSearchFilter, IndividualSearchCourtFilter>();
        services.AddTransient<IIndiviualSearchFilter, IndividualSearchCourtNameFilter>();

        services.AddTransient<ISearchTermFormattingService, SearchTermFormattingService>();
        services.AddTransient<ISearchCleaningService, SearchCleaningService>();

        services.AddScoped(typeof(ITableStorageRepository<>), typeof(AzureTableStorageRepository<>));


    })
    .Build();

host.Run();
