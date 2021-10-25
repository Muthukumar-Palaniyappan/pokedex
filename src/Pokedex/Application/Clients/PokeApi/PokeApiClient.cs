using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pokedex.Api.Application.Clients.PokeApi.Models;
using Pokedex.Api.Application.Exceptions;
using Pokedex.Api.Application.Options;
using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Pokedex.Api.Application.Clients.PokeApi
{
    public class PokeApiClient:IPokeApiClient
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly PokeApiServiceOptions _pokeApiServiceOptions;
        private readonly ILogger<PokeApiClient> _logger;
        public PokeApiClient(IHttpClientFactory clientFactory, IOptions<PokeApiServiceOptions> pokeApiServiceOptions,ILogger<PokeApiClient> logger)
        {
            _clientFactory = clientFactory;
            _pokeApiServiceOptions = pokeApiServiceOptions.Value;
            _logger = logger;
        }

        public async Task<PokemonResponse> GetPokemonAsync(string Name)
        {
            _logger.LogInformation("Fetching Pokemon {@name}.", Name);
            var client = _clientFactory.CreateClient();
            client.BaseAddress = _pokeApiServiceOptions.BaseUri;
            var response = client.GetAsync($"/api/v2/pokemon-species/{Name}").Result;
            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                var pokemonResponse =  await JsonSerializer.DeserializeAsync
                    <PokemonResponse>(responseStream);
                _logger.LogInformation("Pokemon retrieved : {@pokemon}", pokemonResponse);
                return pokemonResponse;
            }
            if (response.StatusCode == HttpStatusCode.NotFound)
                throw new ResourceNotFoundException($"Pokemon {@Name} is not found.");
            var errorMessage = response.Content.ReadAsStringAsync().Result;
            throw new Exception(errorMessage);
        }
    }
}
