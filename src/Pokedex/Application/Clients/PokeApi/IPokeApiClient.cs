using Pokedex.Api.Application.Clients.PokeApi.Models;
using System.Threading.Tasks;

namespace Pokedex.Api.Application.Clients.PokeApi
{
    public interface IPokeApiClient
    {
        public Task<PokemonResponse>  GetPokemonAsync(string Name);
    }
}
