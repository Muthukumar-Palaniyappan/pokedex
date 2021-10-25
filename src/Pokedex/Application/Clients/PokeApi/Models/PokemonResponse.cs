using System.Collections.Generic;

namespace Pokedex.Api.Application.Clients.PokeApi.Models
{
    public class PokemonResponse
    {
        public string name { get; set; }
        public bool is_legendary { get; set; }
        public PokemonHabitat habitat { get; set; }
        public List<FlavorText> flavor_text_entries { get; set; }
    }
}
