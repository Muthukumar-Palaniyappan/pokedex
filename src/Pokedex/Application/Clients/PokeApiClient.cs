using Microsoft.Extensions.Options;
using Pokedex.Api.Application.Clients.Models;
using Pokedex.Api.Application.CustomExceptions;
using Pokedex.Api.Application.Options;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Web;

namespace Pokedex.Api.Application.Clients
{
    public class PokeApiClient:IPokeApiClient
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly PokeApiServiceOptions _pokeApiServiceOptions;
        public PokeApiClient(IHttpClientFactory clientFactory, IOptions<PokeApiServiceOptions> pokeApiServiceOptions)
        {
            _clientFactory = clientFactory;
            _pokeApiServiceOptions = pokeApiServiceOptions.Value;
        }

        public PokemonResponse GetPokemon(string name)
        {
            var client = _clientFactory.CreateClient();
            client.BaseAddress = _pokeApiServiceOptions.BaseUri;
            var response = client.GetAsync($"/api/v2/pokemon-species/{name}").Result;
            if (response.IsSuccessStatusCode)
            {
                using var responseStream =  response.Content.ReadAsStreamAsync().Result;
                return  JsonSerializer.DeserializeAsync
                    <PokemonResponse>(responseStream).Result;
            }
            //TODO: Negative cases
            throw new NotImplementedException();
        }
    }
}
