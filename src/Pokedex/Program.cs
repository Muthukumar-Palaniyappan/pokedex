using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Pokedex.Api.Application.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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
            .ConfigureAppConfiguration((context, configurationBuilder) =>
            {
                configurationBuilder
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", false, true)
                    .AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", true)
                    .AddEnvironmentVariables();
            })
            .ConfigureServices(AddServices);

        private static void AddServices(HostBuilderContext hostContext, IServiceCollection services)
        {
            services.PostConfigure<HostOptions>(option => { option.ShutdownTimeout = TimeSpan.FromSeconds(60); });
            services.ConfigureDependencies();
            services.AddHttpClientsOptions(hostContext.Configuration);
        }


    }
}
