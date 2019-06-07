using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using Funq;
using ServiceStack;
using ServiceStack.Data;
using ServiceStack.OrmLite;

namespace MyApp
{
    public class ConfigureDb : IConfigureServices
    {
        public void Configure(IServiceCollection services)
        {
            services.AddSingleton<IDbConnectionFactory>(
                new OrmLiteConnectionFactory(":memory:", SqliteDialect.Provider));
        }
    }
}
