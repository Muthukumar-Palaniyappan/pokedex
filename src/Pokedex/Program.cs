using Destructurama;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pokedex.Api.Application.Extensions;
using Serilog;
using System;
using System.IO;

namespace Pokedex
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
              .ConfigureLogging((context, logging) =>
              {
                  Log.Logger = new LoggerConfiguration()
                               .ReadFrom.Configuration(context.Configuration)
                               .Destructure.UsingAttributes()
                               .CreateLogger();
                  logging.AddSerilog();
              })
            .ConfigureAppConfiguration((context, configurationBuilder) =>
            {
                configurationBuilder
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", false, true)
                    .AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", true)
                    .AddEnvironmentVariables();
            })
            .UseSerilog()
            .ConfigureServices(AddServices);

        private static void AddServices(HostBuilderContext hostContext, IServiceCollection services)
        {
            services.PostConfigure<HostOptions>(option => { option.ShutdownTimeout = TimeSpan.FromSeconds(60); });
            services.ConfigureDependencies();
            services.AddHttpClientsOptions(hostContext.Configuration);
        }


    }
}
