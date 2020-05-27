using System;
using System.IO;

using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Services.Identity.API.Data;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;

namespace Services.Identity.API
{
    public class Program
    {
        public static int Main(string[] args)
        {

            var configuration = GetConfiguration();
            var host = BuildWebHost(configuration, args);

            host.MigrateDbContext<PersistedGrantDbContext>((_, __) => { })
                  .MigrateDbContext<ApplicationDbContext>((context, services) =>
                  {
                      var env = services.GetService<IHostingEnvironment>();
                      var logger = services.GetService<ILogger<ApplicationDbContextSeed>>();
                      var settings = services.GetService<IOptions<AppSettings>>();

                      new ApplicationDbContextSeed()
                          .SeedAsync(context, env, logger, settings)
                          .Wait();
                  })
                  .MigrateDbContext<ConfigurationDbContext>((context, services) =>
                  {
                      new ConfigurationDbContextSeed()
                          .SeedAsync(context, configuration)
                          .Wait();
                  });

            host.Run();
            return 0;
        }

        private static IWebHost BuildWebHost(IConfiguration configuration, string[] args) =>
    WebHost.CreateDefaultBuilder(args)
        .CaptureStartupErrors(false)
        .UseStartup<Startup>()
        .UseContentRoot(Directory.GetCurrentDirectory())
        .UseConfiguration(configuration)
        .Build();


        private static IConfiguration GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            return builder.Build();
        }
    }
}
