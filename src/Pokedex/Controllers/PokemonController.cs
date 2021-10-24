using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pokedex.Api.Application.Clients;
using Pokedex.Api.Application.Services;
using Pokedex.Contract;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Pokedex.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PokemonController : ControllerBase
    {
        private readonly IPokemonService _pokeApiService;
        public PokemonController(IPokemonService pokeApiService)
        {
            _pokeApiService = pokeApiService;
        }

        /// <summary>
        /// GetPokemon by Name. 
        /// </summary>
        /// <param name="Name"></param>
        /// <returns>Pokemon</returns>
        [HttpGet("{Name}")]
        [ProducesResponseType(typeof(Pokemon), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ActionResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ActionResult), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ActionResult), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Pokemon>> GetPokemon([FromRoute]string Name)
        {
            if (string.IsNullOrEmpty(Name))
                throw new ValidationException("Name shouldn't be empty."); //TODO: RequestObject and implemention of fluent validation. 
            var o =  _pokeApiService.GetPokemon(Name);
            return o;
        }
    }
}
