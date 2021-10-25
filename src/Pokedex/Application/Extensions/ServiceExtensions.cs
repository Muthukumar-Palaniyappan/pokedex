using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pokedex.Api.Application.Clients.PokeApi;
using Pokedex.Api.Application.Clients.Translator;
using Pokedex.Api.Application.Middlewares;
using Pokedex.Api.Application.Options;
using Pokedex.Api.Application.Services;

namespace Pokedex.Api.Application.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddHttpClientsOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<PokeApiServiceOptions>(configuration.GetSection("PokeApiService"));
            services.Configure<TranslatorServiceOptions>(configuration.GetSection("TranslatorService"));
            services.AddHttpClient();
            return services;
        }
        public static void ConfigureDependencies(this IServiceCollection services)
        {
            services.AddScoped<IPokeApiClient, PokeApiClient>();
            services.AddScoped<ITranslatorClient, TranslatorClient>();
            services.AddScoped<IPokemonService, PokemonService>();
            services.AddSingleton<IExceptionHandler, PokeApiExceptionhandler>();
        }

    }
}
