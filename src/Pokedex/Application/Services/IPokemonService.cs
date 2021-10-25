using Pokedex.Contract;
using System.Threading.Tasks;

namespace Pokedex.Api.Application.Services
{
    public interface IPokemonService
    {
        public Task<Pokemon> GetPokemonAsync(string Name, bool translate = false);
    }
}
