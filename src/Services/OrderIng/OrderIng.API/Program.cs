using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OrderIng.API.Infrastructure.Middlewares;
using Serilog;

namespace OrderIng.API
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var configuration = GetConfiguration();
            CreateWebHostBuilder(configuration,args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(IConfiguration configuration, string[] args) =>
             WebHost.CreateDefaultBuilder(args)
              .CaptureStartupErrors(false)
              .UseFailing(options =>
                  options.ConfigPath = "/Failing")
              .UseStartup<Startup>()
              .UseContentRoot(Directory.GetCurrentDirectory())
              .UseConfiguration(configuration)
              .UseSerilog();


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
