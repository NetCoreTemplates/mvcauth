using Funq;
using ServiceStack;
using MyApp.ServiceInterface;
using Microsoft.AspNetCore.Authentication.Cookies;

[assembly: HostingStartup(typeof(MyApp.AppHost))]

namespace MyApp;

public class AppHost : AppHostBase, IHostingStartup
{
    public void Configure(IWebHostBuilder builder) => builder
        .ConfigureServices(services => {
            // Configure ASP.NET Core IOC Dependencies
#if DEBUG
    services.AddMvc(options => options.EnableEndpointRouting = false).AddRazorRuntimeCompilation();
#else
    services.AddMvc(options => options.EnableEndpointRouting = false);
#endif
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme);
        })
        .Configure(app => {
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();

            // Configure ASP.NET Core App
            if (!HasInit)
                app.UseServiceStack(new AppHost());

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        });

    public AppHost() : base("MyApp", typeof(MyServices).Assembly) {}

    public override void Configure(Container container)
    {
        SetConfig(new HostConfig {
                UseSameSiteCookies = true,
#if DEBUG                
                AdminAuthSecret = "adm1nSecret", // Enable Admin Access with ?authsecret=adm1nSecret
#endif
        });
    }
}
