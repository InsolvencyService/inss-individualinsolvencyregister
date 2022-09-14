using AspNetCore.SEOHelper;
using INSS.EIIR.Data.Models;
using INSS.EIIR.DataAccess;
using INSS.EIIR.Interfaces.DataAccess;
using INSS.EIIR.Interfaces.Services;
using INSS.EIIR.Models.Constants;
using INSS.EIIR.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.IdentityModel.Tokens;

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
    services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(authenticationOptions =>
        {
            authenticationOptions.Cookie.Name = "SessionCookie";
            authenticationOptions.SlidingExpiration = true;

            authenticationOptions.Events.OnRedirectToLogin = context =>
            {
                if (IsAdminContext(context))
                {
                    context.Response.Redirect("/Login/Index/AdminLogin");
                }
                else
                {
                    context.Response.Redirect("/Login/Index/SubscriberLogin");
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

    var configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json");

    var config = configuration.Build();

    builder.Services.AddTransient(_ =>
    {
        var connectionString = config.GetConnectionString("iirwebdbContextConnectionString");
        return new EIIRContext(connectionString);
    });

    services.AddTransient<IAuthenticationProvider, AuthenticationProvider>();
    services.AddTransient<IAccountRepository, AccountRepository>();
}

static bool IsAdminContext(RedirectContext<CookieAuthenticationOptions> context)
{
    return context.Request.Path.StartsWithSegments($"/{Role.Admin}");
}