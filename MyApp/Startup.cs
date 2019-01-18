using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Funq;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyApp.ServiceInterface;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using ServiceStack.Text;

namespace MyApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();
            
            app.UseServiceStack(new AppHost
            {
                AppSettings = new NetCoreAppSettings(Configuration)
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
    
    public class AppHost : AppHostBase
    {
        public AppHost() : base("MyApp", typeof(MyServices).Assembly) { }

        // Configure your AppHost with the necessary configuration and dependencies your App needs
        public override void Configure(Container container)
        {
            SetConfig(new HostConfig
            {
                DebugMode = AppSettings.Get(nameof(HostConfig.DebugMode), false),
#if DEBUG                
                AdminAuthSecret = "adm1nSecret", // Enable Admin Access with ?authsecret=adm1nSecret
#endif
            });

            container.Register<IDbConnectionFactory>(c =>
                new OrmLiteConnectionFactory(":memory:", SqliteDialect.Provider));

            container.Register<IAuthRepository>(c =>
                new OrmLiteAuthRepository(c.Resolve<IDbConnectionFactory>()) {
                    UseDistinctRoleTables = true,
                });
            container.Resolve<IAuthRepository>().InitSchema();
            
            // TODO: Replace OAuth App settings in: appsettings.Development.json
            Plugins.Add(new AuthFeature(() => new CustomUserSession(), 
                new IAuthProvider[] {
                    new NetCoreIdentityAuthProvider(AppSettings) { // Adapter to enable ServiceStack Auth in MVC
                        AdminRoles = { "Manager" }, // Automatically Assign additional roles to Admin Users
                    },
                    new CredentialsAuthProvider(AppSettings),     // Sign In with Username / Password credentials 
                    new FacebookAuthProvider(AppSettings), /* Create Facebook App at: https://developers.facebook.com/apps */
                    new TwitterAuthProvider(AppSettings),  /* Create Twitter App at: https://dev.twitter.com/apps */
                    new GoogleAuthProvider(AppSettings),   /* Create App https://console.developers.google.com/apis/credentials */
                    new MicrosoftGraphAuthProvider(AppSettings),   /* Create App https://apps.dev.microsoft.com */
                }) {
                IncludeRegistrationService = true,
                IncludeAssignRoleServices = false,
            });

            AddSeedUsers((IUserAuthRepository)container.Resolve<IAuthRepository>());
        }

        private void AddSeedUsers(IUserAuthRepository authRepo)
        {
            if (authRepo.GetUserAuthByUserName("user@gmail.com") == null)
            {
                var testUser = authRepo.CreateUserAuth(new UserAuth
                {
                    DisplayName = "Test User",
                    Email = "user@gmail.com",
                    FirstName = "Test",
                    LastName = "User",
                }, "p@55wOrd");
            }

            if (authRepo.GetUserAuthByUserName("manager@gmail.com") == null)
            {
                var roleUser = authRepo.CreateUserAuth(new UserAuth
                {
                    DisplayName = "Test Manager",
                    Email = "manager@gmail.com",
                    FirstName = "Test",
                    LastName = "Manager",
                }, "p@55wOrd");
                authRepo.AssignRoles(roleUser, roles:new[]{ "Manager" });
            }

            if (authRepo.GetUserAuthByUserName("admin@gmail.com") == null)
            {
                var roleUser = authRepo.CreateUserAuth(new UserAuth
                {
                    DisplayName = "Admin User",
                    Email = "admin@gmail.com",
                    FirstName = "Admin",
                    LastName = "User",
                }, "p@55wOrd");
                authRepo.AssignRoles(roleUser, roles:new[]{ "Admin" });
            }
        }
    }

    // Add any additional metadata properties you want to store in the Users Typed Session
    public class CustomUserSession : AuthUserSession
    {
    }
        
}
