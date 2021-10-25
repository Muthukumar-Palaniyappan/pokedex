using AutoMapper;
using Microsoft.Extensions.Logging;
using Pokedex.Api.Application.Clients.PokeApi;
using Pokedex.Api.Application.Clients.PokeApi.Models;
using Pokedex.Api.Application.Clients.Translator;
using Pokedex.Contract;
using System;
using System.Threading.Tasks;

namespace Pokedex.Api.Application.Services
{
    public class PokemonService : IPokemonService
    {
        private readonly IPokeApiClient _pokeApiClient;
        private readonly ITranslatorClient _translatorClient;
        private readonly IMapper _mapper;
        private readonly ILogger<PokemonService> _logger;

        private const string PokemonHabitatCave = "cave";
        
        public PokemonService(IPokeApiClient pokeApiClient, ITranslatorClient translatorClient,IMapper mapper, ILogger<PokemonService> logger)
        {
            _pokeApiClient = pokeApiClient;
            _translatorClient = translatorClient;
            _logger = logger;
            _mapper = mapper;
         }

        public async Task<Pokemon> GetPokemonAsync(string Name, bool Translate = false)
        {
            _logger.LogInformation("Fetching Pokemon {@name} - {@translate}", Name,Translate);
            var pokemonResponse = await _pokeApiClient.GetPokemonAsync(Name);
            var pokemon = _mapper.Map<PokemonResponse,Pokemon>(pokemonResponse);

            if (Translate)
            {
                pokemon.Description = (pokemon.Habitat.Equals(PokemonHabitatCave, StringComparison.OrdinalIgnoreCase) || pokemon.IsLegendary) ?
                                        await _translatorClient.GetTranslationAsync(pokemon.Description,TranslationType.Yoda):
                                        await _translatorClient.GetTranslationAsync(pokemon.Description,TranslationType.Shakespeare);
            }
            _logger.LogInformation("Pokemon retrieved {@name} - {@translate} - {@pokemon}", Name, Translate,pokemon);
            return pokemon;
        }
    }
}
