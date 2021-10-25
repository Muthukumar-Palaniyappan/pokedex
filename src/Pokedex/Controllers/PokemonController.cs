using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pokedex.Api.Application.Clients.PokeApi;
using Pokedex.Api.Application.Exceptions;
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
        [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPokemonAsync([FromRoute]string Name)
        {
            if (string.IsNullOrEmpty(Name))
                throw new ValidationException("Name shouldn't be empty."); //TODO: RequestObject and implemention of fluent validation. 
           return Ok(await _pokeApiService.GetPokemonAsync(Name));
        }
        
        /// <summary>
        /// GetPokemon and its translated description by Name. 
        /// </summary>
        /// <param name="Name"></param>
        /// <returns>Pokemon</returns>
        [HttpGet("translated/{Name}")]
        [ProducesResponseType(typeof(Pokemon), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPokemonTranslatedAsync([FromRoute] string Name)
        {
            if (string.IsNullOrEmpty(Name))
                throw new ValidationException("Name shouldn't be empty."); 
            return Ok(await _pokeApiService.GetPokemonAsync(Name,true));
        }
    }
}
