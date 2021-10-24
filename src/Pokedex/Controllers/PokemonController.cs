﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pokedex.Api.Application.Clients;
using Pokedex.Contract;
using System.Threading.Tasks;

namespace Pokedex.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PokemonController : ControllerBase
    {
        private readonly IPokeApiClient _pokeApiClient;
        public PokemonController(IPokeApiClient pokeApiClient)
        {
            _pokeApiClient =  pokeApiClient;
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
            await Task.Delay(10);
            _pokeApiClient.GetPokemon(Name);
            return Ok(new Pokemon());
        }
    }
}