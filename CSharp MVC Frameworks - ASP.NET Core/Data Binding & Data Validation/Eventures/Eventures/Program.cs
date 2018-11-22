using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Eventures
{
    using Eventures.Loggers;

    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();

                    var config = new ColoredConsoleLoggerConfiguration
                    {
                        LogLevel = LogLevel.Information,
                        Color = ConsoleColor.Green
                    };
                    logging.AddProvider(new ColoredConsoleLoggerProvider(config));
                })
                .UseStartup<Startup>()
                .Build();
    }
}
