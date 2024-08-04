using System.Net;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using SocialNexusClient.Services;

namespace SocialNexusClient;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
}

public class Startup
{
    public Startup(IConfiguration configuration, IWebHostEnvironment env)
    {
        Configuration = configuration;
        CurrentEnvironment = env;
    }

    private IWebHostEnvironment CurrentEnvironment { get; set; }
    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services
            // default authentication scheme
            .AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
            // Sign-in users with the Microsoft identity platform
            .AddMicrosoftIdentityWebApp(o =>
            {
                // pass configuration settings for SocialNexus AD B2C
                Configuration.Bind("AzureAdB2C", o);

                // create a new event that gets triggered by OpenId
                o.Events ??= new OpenIdConnectEvents();

                // if in production
                if (CurrentEnvironment.IsProduction())
                    // hardcode the redirect URI
                    o.Events.OnRedirectToIdentityProvider += SetRedirectURI;
            })

        services.AddControllersWithViews().AddMicrosoftIdentityUI();


        services.AddRazorPages();

    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseHttpsRedirection();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }
        app.UseStaticFiles();
        app.UseCookiePolicy();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseStaticFiles();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}"
            );
            endpoints.MapRazorPages();
        });
    }

    // sets where our user gets sent after logging in
    public async Task SetRedirectURI(RedirectContext context)
    {
        // send the user to the production site
        context.ProtocolMessage.RedirectUri = "https://social-nexus.net/signin-oidc";

        await Task.CompletedTask.ConfigureAwait(false);
    }
}
