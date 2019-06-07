using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using ServiceStack.Configuration;

namespace MyApp
{
    [Priority(-80)] // Run before AppHost.Configure()
    public class ConfigureAuthRepository : IConfigureAppHost, IConfigureServices
    {
        public void Configure(IServiceCollection services)
        {
            services.AddSingleton<IAuthRepository>(c =>
                new OrmLiteAuthRepository(c.Resolve<IDbConnectionFactory>()) {
                    UseDistinctRoleTables = true,
                });
        }

        public void Configure(IAppHost appHost)
        {
            var authRepo = (IUserAuthRepository)appHost.Resolve<IAuthRepository>();
            authRepo.InitSchema();
            AddSeedUsers(authRepo);
        }

        // Add initial Users to the configured Auth Repository
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
}
