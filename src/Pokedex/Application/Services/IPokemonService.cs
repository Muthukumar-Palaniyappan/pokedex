using Pokedex.Contract;

namespace Pokedex.Api.Application.Services
{
    public interface IPokemonService
    {
        public Pokemon GetPokemon(string Name, bool translate = false);
    }
}
