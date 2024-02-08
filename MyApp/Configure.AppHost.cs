using Funq;
using ServiceStack;
using MyApp.ServiceInterface;

[assembly: HostingStartup(typeof(MyApp.AppHost))]

namespace MyApp;

public class AppHost() : AppHostBase("MyApp", typeof(MyServices).Assembly), IHostingStartup
{
    public void Configure(IWebHostBuilder builder) => builder
        .ConfigureServices(services => {
            // Configure ASP.NET Core IOC Dependencies
        });

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
