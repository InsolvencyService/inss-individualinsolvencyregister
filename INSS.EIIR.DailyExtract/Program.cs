using AutoMapper;

using Microsoft.Extensions.Azure;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using INSS.EIIR.DataSync.Application.UseCase.SyncData.Model;
using INSS.EIIR.DataSync.Application;
using INSS.EIIR.DataSync.Functions.DI;
using INSS.EIIR.DataSync.Application.UseCase.SyncData;
using INSS.EIIR.Models.AutoMapperProfiles;
using INSS.EIIR.AzureSearch.IndexMapper;
using INSS.EIIR.DataSync.Infrastructure.Sink.XML;
using INSS.EIIR.Interfaces.DataAccess;
using INSS.EIIR.DataAccess;
using INSS.EIIR.Models.Configuration;
using Microsoft.Extensions.Configuration;
using INSS.EIIR.Data.Models;
using Microsoft.EntityFrameworkCore;
using INSS.EIIR.DataSync.Application.UseCase.SyncData.Validation;
using INSS.EIIR.Models.SyncData;
using INSS.EIIR.DataSync.Application.UseCase.SyncData.Infrastructure;
using INSS.EIIR.DataSync.Application.UseCase.SyncData.Transformation;



var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services => {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();

        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        services.AddSetIndexMapper(new IndexMapperOptions()
        {
            TableStorageConnectionString = Environment.GetEnvironmentVariable("TableStorageConnectionString")
        });

        services.AddExistingBankrupticesService(new ExistingBankruptciesOptions()
        {
            BlobStorageConnectionString = Environment.GetEnvironmentVariable("TargetBlobConnectionString"),
            
            //defaults for following items are set, if they do not exist in config
            BlobStorageContainer = Environment.GetEnvironmentVariable("ExistingBankruptciesContainer"),
            ExistingBankruptciesFileName = Environment.GetEnvironmentVariable("ExistingBankruptciesFile")
        });

        // Auto Mapper Configurations
        var mapperConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new IndividualSearchMapper());
            mc.AddProfile(new ExtractMapper());
            mc.AddProfile(new INSS.EIIR.DataSync.Infrastructure.Source.SQL.Models.AutoMapperProfiles.InsolventIndividualRegisterModelMapper());
            mc.AddProfile(new INSS.EIIR.DataSync.Application.UseCase.SyncData.AutoMapperProfiles.InsolventIndividualRegisterModelMapper());
        });

        var mapper = mapperConfig.CreateMapper();
        services.AddSingleton(mapper);

        services.AddAzureClients(azureBuilder =>
        {
            azureBuilder.AddBlobServiceClient(Environment.GetEnvironmentVariable("BlobStorageConnectionString"));
        });

        var connectionString = Environment.GetEnvironmentVariable("database__connectionstring");
        if (string.IsNullOrEmpty(connectionString))
            throw new ArgumentNullException("database__connectionstring missing");

        //Required for ExtractRepository
        services.AddTransient(_ =>
        {
            return new EIIRContext(connectionString);
        });

        //Required for ExtractRepository
        services.AddDbContext<EIIRExtractContext>(options => options.UseSqlServer(connectionString));

        services.AddOptions<DatabaseConfig>()
           .Configure<IConfiguration>((settings, configuration) =>
           {
               configuration.GetSection("database").Bind(settings);
           });

        services.AddScoped<IExtractRepository, ExtractRepository>();

        services.AddTransient<IValidationRule, IdValidationRule>();
        services.AddTransient<IValidationRule, AliasValidationRule>();
        services.AddTransient<IValidationRule, TradingNamesValidationRule>();
        services.AddTransient<IValidationRule, RestrictionsTypeValidationRule>();
        services.AddTransient<IValidationRule, RestrictionValidationRule>();

        services.AddTransient<ITransformRule, AliasTransformRule>();
        services.AddTransient<ITransformRule, RestrictionsTypeTransformRule>();
        services.AddTransient<ITransformRule, AddressTransformRule>();

        services.AddScoped<IRequestResponseUseCase<SyncDataRequest, SyncDataResponse>, SyncData>(SyncDataFactory.Get);
        
    })
    .Build();

host.Run();


