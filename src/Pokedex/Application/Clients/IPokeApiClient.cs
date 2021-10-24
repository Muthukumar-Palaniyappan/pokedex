using Pokedex.Api.Application.Clients.Models;

namespace Pokedex.Api.Application.Clients
{
    public interface IPokeApiClient
    {
        public PokemonResponse  GetPokemon(string Name);
    }
}
