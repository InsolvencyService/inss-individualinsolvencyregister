using AspNetCore.SEOHelper;
using Microsoft.AspNetCore.Rewrite;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
ConfigureServices(builder.Services);

var app = builder.Build();




// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
   
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseRobotsTxt(app.Environment.ContentRootPath);

var options = new RewriteOptions()
    .AddRedirect("security.txt$", @"https://security.insolvency.gov.uk/.well-known/security.txt");

app.UseRewriter(options);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

void ConfigureServices(IServiceCollection services)
{
    services.AddControllersWithViews();
}