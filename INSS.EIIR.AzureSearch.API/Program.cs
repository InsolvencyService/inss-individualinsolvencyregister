using AutoMapper;
using Azure.Search.Documents.Indexes;
using Azure;
using INSS.EIIR.AzureSearch.API.Configuration;
using INSS.EIIR.AzureSearch.Services;
using INSS.EIIR.AzureSearch.Services.ODataFilters;
using INSS.EIIR.Interfaces.AzureSearch;
using INSS.EIIR.Models.AutoMapperProfiles;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Newtonsoft.Json;
using INSS.EIIR.Models.HealthModels;
using INSS.EIIR.Data.Models;
using Microsoft.Extensions.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

ConfigureServices(builder.Services);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

app.UseHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        var response = new HealthCheckReponse
        {
            Status = report.Status.ToString(),
            HealthChecks = report.Entries.Select(x => new IndividualHealthCheckResponse
            {
                Component = x.Key,
                Status = x.Value.Status.ToString(),
                Description = x.Value.Description
            }),
            HealthCheckDuration = report.TotalDuration
        };
        await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
    }
});

app.UseHealthChecks("/ping", new HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        var response = new HealthCheckReponse
        {
            Status = report.Status.ToString(),
            
        };
        await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
    }
});

void ConfigureServices(IServiceCollection services)
{
    // Add services to the container.
    services.AddTransient<IIndividualQueryService, INSS.EIIR.AzureSearch.Services.QueryServices.IndividualQueryService>();
    
    services.AddTransient<IIndiviualSearchFilter, IndividualSearchCourtFilter>();
    services.AddTransient<IIndiviualSearchFilter, IndividualSearchCourtNameFilter>();

    services.AddTransient<ISearchTermFormattingService, SearchTermFormattingService>();
    services.AddTransient<ISearchCleaningService, SearchCleaningService>();

    services.AddControllers();

    // Auto Mapper Configurations
    var mapperConfig = new MapperConfiguration(mc =>
    {
        mc.AddProfile(new IndividualSearchMapper());
    });

    var mapper = mapperConfig.CreateMapper();
    services.AddSingleton(mapper);

    var settings = new SettingOptions();
    configuration.GetSection(SettingOptions.Settings).Bind(settings);

    builder.Services.AddTransient(_ =>
    {
        var searchServiceUrl = settings.EIIRIndexUrl;
        var adminApiKey = settings.EIIRApiKey;

        return CreateSearchServiceClient(searchServiceUrl, adminApiKey);
    });

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();
    services.AddHealthChecks().AddCheck("ping", () => new HealthCheckResult(HealthStatus.Healthy, "Healthy"), new string[] { "200" }).AddDbContextCheck<EIIRContext>().AddUrlGroup(new Uri("DRO_API_HEALTH_ENDPOINT"));


}

static SearchIndexClient CreateSearchServiceClient(string searchServiceUrl, string adminApiKey)
{
    var serviceClient = new SearchIndexClient(new Uri(searchServiceUrl), new AzureKeyCredential(adminApiKey));

    return serviceClient;
}