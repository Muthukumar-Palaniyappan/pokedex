using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pokedex.Api.Application.Clients.Models;
using Pokedex.Api.Application.Exceptions;
using Pokedex.Api.Application.Options;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Http;
using System.Text.Json;

namespace Pokedex.Api.Application.Clients
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

        public PokemonResponse GetPokemon(string name)
        {
            _logger.LogInformation("Fetching Pokemon {@name}.", name);
            var client = _clientFactory.CreateClient();
            client.BaseAddress = _pokeApiServiceOptions.BaseUri;
            var response = client.GetAsync($"/api/v2/pokemon-species/{name}").Result;
            if (response.IsSuccessStatusCode)
            {
                using var responseStream =  response.Content.ReadAsStreamAsync().Result;
                return  JsonSerializer.DeserializeAsync
                    <PokemonResponse>(responseStream).Result;
            }
            if (response.StatusCode == HttpStatusCode.NotFound)
                throw new ResourceNotFoundException($"Pokemon {@name} is not found.");
            var errorMessage = response.Content.ReadAsStringAsync().Result;
            throw new Exception(errorMessage);
        }
    }
}
