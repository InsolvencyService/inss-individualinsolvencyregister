using Microsoft.Extensions.Azure;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.SqlServer.Management.Smo.Wmi;
using System;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services => {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        services.AddAzureClients(azureBuilder =>
        {
            azureBuilder.AddBlobServiceClient(Environment.GetEnvironmentVariable("BlobStorageConnectionString"));
        });
    })
    .Build();

host.Run();


