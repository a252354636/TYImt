﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace WebMVC
{
    public class Program
    {
        public static int Main(string[] args)
        {
            var configuration = GetConfiguration();
            var host = BuildWebHost(configuration, args);
            host.Run();

            return 0;
        }

        private static IWebHost BuildWebHost(IConfiguration configuration, string[] args) =>
                  WebHost.CreateDefaultBuilder(args)
                      .CaptureStartupErrors(false)
                      .UseStartup<Startup>()
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
