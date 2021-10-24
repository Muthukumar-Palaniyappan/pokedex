using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pokedex.Api.Application.Clients;
using Pokedex.Api.Application.Middlewares;
using Pokedex.Api.Application.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pokedex.Api.Application.Extensions
{
    public static class HttpClientsServiceExtensions
    {
        public static IServiceCollection AddHttpClientsOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<PokeApiServiceOptions>(configuration.GetSection("PokeApiService"));
            services.Configure<YodaTransalatorServiceOptions>(configuration.GetSection("YodaTransalatorService"));
            services.Configure<ShakespeareTransalatorServiceOptions>(configuration.GetSection("ShakespeareTransalatorService"));
            services.AddHttpClient();
            return services;
        }
        public static void ConfigureDependencies(this IServiceCollection services)
        {
            services.AddScoped<IPokeApiClient, PokeApiClient>();
            services.AddSingleton<IExceptionHandler, PokeApiExceptionhandler>();

        }

        //public static IServiceCollection RegisterHttpClients(this IServiceCollection services)
        //{
        //    RegisterCustomerCoreServiceClient(services);
        //    RegisterAccountServiceClient(services);
        //    return services;
        //}

        //private static void RegisterCustomerCoreServiceClient(IServiceCollection services)
        //{
        //    services.AddSingleton<CustomerCoreHttpClientFactory<ICustomerCoreServiceClient>>();
        //    services.AddHttpClient<ICustomerCoreServiceClient, CustomerCoreServiceClient>((serviceProvider, client) =>
        //    {
        //        var customerCoreServiceOptions =
        //            serviceProvider.GetService<IOptions<CustomerCoreServiceOptions>>().Value;
        //        client.BaseAddress = customerCoreServiceOptions.BaseUri;
        //        client.DefaultRequestHeaders.Add(ApplicationConstants.XRequestId,
        //                                         CorrelationIdContext.GetCorrelationId() ??
        //                                         Guid.NewGuid().ToString());
        //    });
        //}

        //private static void RegisterAccountServiceClient(IServiceCollection services)
        //{
        //    services.AddSingleton<CustomerCoreHttpClientFactory<IAccountServiceClient>>();
        //    services.AddHttpClient<IAccountServiceClient, AccountServiceClient>((serviceProvider, client) =>
        //    {
        //        var customerCoreServiceOptions =
        //            serviceProvider.GetService<IOptions<AccountServiceOptions>>().Value;
        //        client.BaseAddress = customerCoreServiceOptions.BaseUri;
        //    });
        //}
    }
}
