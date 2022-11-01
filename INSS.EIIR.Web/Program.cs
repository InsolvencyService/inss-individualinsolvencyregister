using AspNetCore.SEOHelper;
using AutoMapper;
using INSS.EIIR.Data.AutoMapperProfiles;
using Flurl.Http;
using INSS.EIIR.Data.Models;
using INSS.EIIR.DataAccess;
using INSS.EIIR.Interfaces.DataAccess;
using INSS.EIIR.Interfaces.Services;
using INSS.EIIR.Interfaces.Web.Services;
using INSS.EIIR.Models.Configuration;
using INSS.EIIR.Models.Constants;
using INSS.EIIR.Services;
using INSS.EIIR.Web.Configuration;
using INSS.EIIR.Web.Services;
using Joonasw.AspNetCore.SecurityHeaders;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Rewrite;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
ConfigureServices(builder.Services);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseStatusCodePagesWithReExecute("/errors", "?statusCode={0}");

    app.UseExceptionHandler("/Errors");

// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseRobotsTxt(app.Environment.ContentRootPath);

app.UseCsp(csp =>
{
    csp.ByDefaultAllow.FromNowhere();

    csp.AllowScripts
        .FromSelf()
        .AddNonce();
    csp.AllowStyles
        .FromSelf();
    csp.AllowFonts
        .FromSelf();
    csp.AllowImages
        .FromSelf();

    if (app.Environment.IsDevelopment())
    {
        //Allows hot-reload script to work in dev only
        csp.AllowConnections
            .To("wss:");
    }

    csp.AllowConnections
        .ToSelf();
});

app.UseCors();

var options = new RewriteOptions()
    .AddRedirect("security.txt$", @"https://security.insolvency.gov.uk/.well-known/security.txt");

app.UseRewriter(options);
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

void ConfigureServices(IServiceCollection services)
{
    services.AddCsp(nonceByteAmount: 32);

    services.AddCors();

    services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(authenticationOptions =>
        {
            authenticationOptions.Cookie.Name = "SessionCookie";
            authenticationOptions.SlidingExpiration = true;

            authenticationOptions.Events.OnRedirectToLogin = context =>
            {
                if (IsAdminContext(context))
                {
                    context.Response.Redirect("/Admin");
                }

                return Task.CompletedTask;
            };
        });

    services.AddAntiforgery(options =>
    {
        // Set Cookie properties using CookieBuilder properties†.
        options.FormFieldName = "AntiforgeryFieldname";
        options.HeaderName = "X-CSRF-TOKEN-HEADERNAME";
        options.SuppressXFrameOptionsHeader = false;
    });

    services.AddControllersWithViews();

    FlurlHttp.Configure(settings =>
    {
        settings.HttpClientFactory = new PollyHttpClientFactory();
    });

    var config = builder.Configuration;

    builder.Services.AddOptions<ApiSettings>()
        .Configure<IConfiguration>((settings, configuration) =>
        {
            configuration.GetSection("ApiSettings").Bind(settings);
        });

    //var appUrl = config.GetConnectionString("EIIRWEB_API_HEALTH_ENDPOINT_HERE");
    //builder.Services.AddHealthChecks().AddUrlGroup(new Uri(appUrl));

    builder.Services.AddTransient(_ =>
    {
        var connectionString = config.GetConnectionString("iirwebdbContextConnectionString");
        return new EIIRContext(connectionString);
    });

    services.AddTransient<IAuthenticationProvider, AuthenticationProvider>();
    services.AddTransient<IAccountRepository, AccountRepository>();

    // Auto Mapper Configurations
    var mapperConfig = new MapperConfiguration(mc =>
    {
        mc.AddProfile(new SubscriberMapper());
    });

    var mapper = mapperConfig.CreateMapper();
    builder.Services.AddSingleton(mapper);

    services.AddTransient<IClientService, ClientService>();
    services.AddTransient<IIndividualSearch, IndividualSearch>();
    services.AddTransient<ISubscriberService, SubscriberService>();
    services.AddTransient<ISubscriberSearch, SubscriberSearch>();
}

static bool IsAdminContext(RedirectContext<CookieAuthenticationOptions> context)
{
    return context.Request.Path.StartsWithSegments($"/{Role.Admin}");
}
