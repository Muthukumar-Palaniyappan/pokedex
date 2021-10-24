using AutoMapper;
using Pokedex.Api.Application.Clients.Models;
using Pokedex.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pokedex.Api.Application.Mapper
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<PokemonResponse, Pokemon>()
                .ForMember(d => d.IsLegendary, opt => opt.MapFrom(s => s.is_legendary))
                .ForMember(d => d.Habitat, opt => opt.MapFrom(s => s.habitat.name))
                .ForMember(d => d.Description, opt => opt.MapFrom(s => s.flavor_text_entries.FirstOrDefault(t => t.language.name.Equals("en", StringComparison.OrdinalIgnoreCase)).flavor_text));
        }
    }
}
