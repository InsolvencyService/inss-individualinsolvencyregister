using AutoMapper;
using INSS.EIIR.AzureSearch.AutoMapperProfiles;
using INSS.EIIR.AzureSearch.Services;
using INSS.EIIR.AzureSearch.Services.ODataFilters;
using INSS.EIIR.AzureSearch.Services.QueryServices;
using INSS.EIIR.Interfaces.AzureSearch;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

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


void ConfigureServices(IServiceCollection services)
{
    // Add services to the container.
    services.AddTransient<IIndividualQueryService, IndividualQueryService>();
    
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

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();
}