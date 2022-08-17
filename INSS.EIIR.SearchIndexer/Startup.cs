using System;
using System.Diagnostics.CodeAnalysis;
using INSS.EIIR.Data.Models;
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
            var config = builder.GetContext().Configuration;

            //builder.Services.AddApplicationInsightsTelemetry();

            builder.Services.AddHttpClient();
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            builder.Services.AddTransient((s) =>
            {
                var connectionString = Environment.GetEnvironmentVariable("iirwebdbContextConnectionString");
                return new iirwebdbContext(connectionString);
            });

            //builder.Services.AddTransient<IAuthBodyService, AuthBodyService>();
            //builder.Services.AddTransient<IInsolvencyPractitionerService, InsolvencyPractitionerService>();
            //builder.Services.AddTransient<IWebMessageService, WebMessageService>();
        }
    }
}
