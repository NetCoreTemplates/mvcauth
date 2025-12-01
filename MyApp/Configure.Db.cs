using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceStack;
using ServiceStack.Data;
using ServiceStack.DataAnnotations;
using ServiceStack.OrmLite;

[assembly: HostingStartup(typeof(MyApp.ConfigureDb))]

namespace MyApp
{
    // Example Data Model
    // public class MyTable
    // {
    //     [AutoIncrement]
    //     public int Id { get; set; }
    //     public string Name { get; set; }
    // }

    public class ConfigureDb : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder) => builder
            .ConfigureServices((context, services) => {
                var connectionString = context.Configuration.GetConnectionString("DefaultConnection")
                    ?? "DataSource=App_Data/app.db;Cache=Shared";
                
                services.AddOrmLite(options => options.UseSqlite(connectionString));
            })
            .ConfigureAppHost(afterConfigure:appHost => {
                appHost.ScriptContext.ScriptMethods.Add(new DbScriptsAsync());

                // Create non-existing Table and add Seed Data Example
                // using var db = appHost.Resolve<IDbConnectionFactory>().Open();
                // if (db.CreateTableIfNotExists<MyTable>())
                // {
                //     db.Insert(new MyTable { Name = "Seed Data for new MyTable" });
                // }
            });
    }
}
