using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using WireMock.Server;
using Microsoft.Extensions.DependencyInjection;

namespace Pokedex.Api.IntegrationTests
{
    public class IntegrationTestFactory<TStartup>
        :WebApplicationFactory<TStartup> where TStartup : class
    {

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            WireMockServers.mockPokedexServer = WireMockServer.Start();
            WireMockServers.mockTranslatorServer = WireMockServer.Start();

            builder
            .ConfigureAppConfiguration(configurationBuilder =>
            {
                configurationBuilder.AddInMemoryCollection(new KeyValuePair<string, string>[]
                    {
                        new KeyValuePair<string, string>("PokeApiService:BaseUri",WireMockServers.mockPokedexServer.Urls[0]),
                        new KeyValuePair<string, string>("TranslatorService:BaseUri",WireMockServers.mockTranslatorServer.Urls[0])
                    });
            });

        }
    }
}
