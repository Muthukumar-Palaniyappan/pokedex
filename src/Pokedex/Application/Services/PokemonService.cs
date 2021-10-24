using AutoMapper;
using Microsoft.Extensions.Logging;
using Pokedex.Api.Application.Clients;
using Pokedex.Api.Application.Clients.Models;
using Pokedex.Contract;

namespace Pokedex.Api.Application.Services
{
    public class PokemonService : IPokemonService
    {
        private readonly IPokeApiClient _pokeApiClient;
        private readonly ITranslatorClient _translatorClient;
        private readonly IMapper _mapper;
        private readonly ILogger<PokemonService> _logger;
        
        public PokemonService(IPokeApiClient pokeApiClient, ITranslatorClient translatorClient,IMapper mapper, ILogger<PokemonService> logger)
        {
            _pokeApiClient = pokeApiClient;
            _translatorClient = translatorClient;
            _logger = logger;
            _mapper = mapper;
         }
        public Pokemon GetPokemon(string Name, bool translate = false)
        {
            var pokemonResponse = _pokeApiClient.GetPokemon(Name);
            var pokemon = _mapper.Map<PokemonResponse,Pokemon>(pokemonResponse);
            return pokemon;
        }
    }
}
