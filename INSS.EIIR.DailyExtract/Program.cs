using AutoMapper;

using Microsoft.Extensions.Azure;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.SqlServer.Management.Smo.Wmi;
using System;
using INSS.EIIR.DataSync.Application.UseCase.SyncData.Model;
using INSS.EIIR.DataSync.Application;
using INSS.EIIR.DataSync.Functions.DI;
using INSS.EIIR.DataSync.Application.UseCase.SyncData;
using INSS.EIIR.Models.AutoMapperProfiles;
using INSS.EIIR.DataSync.Application.UseCase.SyncData.AutoMapperProfiles;
using INSS.EIIR.DataSync.Infrastructure.Source.SQL.Models.AutoMapperProfiles;
using INSS.EIIR.AzureSearch.IndexMapper;

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

        // Auto Mapper Configurations
        var mapperConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new IndividualSearchMapper());
            mc.AddProfile(new INSS.EIIR.DataSync.Infrastructure.Source.SQL.Models.AutoMapperProfiles.InsolventIndividualRegisterModelMapper());
            mc.AddProfile(new INSS.EIIR.DataSync.Application.UseCase.SyncData.AutoMapperProfiles.InsolventIndividualRegisterModelMapper());
        });

        var mapper = mapperConfig.CreateMapper();
        services.AddSingleton(mapper);

        services.AddAzureClients(azureBuilder =>
        {
            azureBuilder.AddBlobServiceClient(Environment.GetEnvironmentVariable("BlobStorageConnectionString"));
        });

        services.AddScoped<IResponseUseCase<SyncDataResponse>, SyncData>(SyncDataFactory.Get);
    })
    .Build();

host.Run();


