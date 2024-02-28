using INSS.EIIR.DailyExtract;

using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

[assembly: FunctionsStartup(typeof(Startup))]
namespace INSS.EIIR.DailyExtract
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var services = builder.Services;
            var configuration = builder.GetContext().Configuration;

            services.AddAzureClients(azureBuilder =>
            {
                azureBuilder.AddBlobServiceClient(Environment.GetEnvironmentVariable("BlobStorageConnectionString"));
            });

            //services.AddAzureClients(clientsBuilder =>
            //{
            //    clientsBuilder.AddServiceBusClient(Environment.GetEnvironmentVariable("ServicebusConnectionString"))
            //        .WithName("ServiceBusSender")
            //        .ConfigureOptions(options =>
            //        {
            //            options.TransportType = ServiceBusTransportType.AmqpWebSockets;
            //        });

            //    clientsBuilder.AddServiceBusClient(Environment.GetEnvironmentVariable("NotifyOptions__ConnectionString"))
            //        .WithName("ServiceBusSenderNotify")
            //        .ConfigureOptions(options =>
            //        {
            //            options.TransportType = ServiceBusTransportType.AmqpWebSockets;
            //        });

            //    clientsBuilder
            //    .AddBlobServiceClient(Environment.GetEnvironmentVariable("BlobConnectionString"));
            //});

        }
    }
}