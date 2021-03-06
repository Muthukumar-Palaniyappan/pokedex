using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Pokedex.Api.Application.Clients.PokeApi;
using Pokedex.Api.Application.Clients.PokeApi.Models;
using Pokedex.Api.Application.Clients.Translator;
using Pokedex.Api.Application.Mapper;
using Pokedex.Api.Application.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Pokedex.Api.UnitTests.Application.Services
{
    public class PokemonServiceShould
    {
        private readonly PokemonService _pokemonService;
        private readonly Mock<IPokeApiClient> _pokeApiClientMock;
        private readonly Mock<ITranslatorClient> _translatorClientMock;
        private readonly Mock<ILogger<PokemonService>> _loggerMock;
        private readonly IMapper _mapper;

        public PokemonServiceShould()
        {
            var config = new MapperConfiguration(opts =>
            {
                opts.AddProfile<MappingProfile>();
            });
            _pokeApiClientMock = new Mock<IPokeApiClient>();
            _translatorClientMock = new Mock<ITranslatorClient>();
            _loggerMock = new Mock<ILogger<PokemonService>>();
            _mapper = config.CreateMapper();
            _pokemonService = new PokemonService(_pokeApiClientMock.Object, _translatorClientMock.Object, _mapper, _loggerMock.Object);
        }
        [Fact]
        public  async Task ReturnsPokemon_WhenGetPokemonRequest_WithValidData()
        {
            //Arrange
            _pokeApiClientMock.Setup(x => x.GetPokemonAsync("test")).ReturnsAsync(GetValidPokemonResponse());
            //Act
            var result =  await _pokemonService.GetPokemonAsync("test");
            //Assert
            result.IsLegendary.Should().BeFalse();
            result.Habitat.Should().Be("rare");
        }

        [Fact]
        public async Task ReturnsYodaTranslatedPokemon_WhenGetPokemonRequest_IsLegendary()
        {
            //Arrange
            var apiResponseObj = GetValidPokemonResponse();
            apiResponseObj.is_legendary = true;
            _pokeApiClientMock.Setup(x => x.GetPokemonAsync("test")).ReturnsAsync(apiResponseObj);
            _translatorClientMock.Setup(y => y.GetTranslationAsync(It.IsAny<string>(), TranslationType.Yoda)).ReturnsAsync("Yoda Translated Text");
            _translatorClientMock.Setup(y => y.GetTranslationAsync(It.IsAny<string>(), TranslationType.Shakespeare)).ReturnsAsync("Shakespeare Translated Text");
            //Act
            var result = await _pokemonService.GetPokemonAsync("test",true);
            //Assert
            result.IsLegendary.Should().BeTrue();
            result.Habitat.Should().Be("rare");
            result.Description.Should().Be("Yoda Translated Text");
        }
        [Fact]
        public async Task ReturnsShakespeareTranslatedPokemon_WhenGetPokemonRequest_IsNotLegendaryAndRare()
        {
            //Arrange
            var apiResponseObj = GetValidPokemonResponse();
            _pokeApiClientMock.Setup(x => x.GetPokemonAsync("test")).ReturnsAsync(apiResponseObj);
            _translatorClientMock.Setup(y => y.GetTranslationAsync(It.IsAny<string>(), TranslationType.Yoda)).ReturnsAsync("Yoda Translated Text");
            _translatorClientMock.Setup(y => y.GetTranslationAsync(It.IsAny<string>(), TranslationType.Shakespeare)).ReturnsAsync("Shakespeare Translated Text");
            //Act
            var result = await _pokemonService.GetPokemonAsync("test", true);
            //Assert
            result.IsLegendary.Should().BeTrue();
            result.Habitat.Should().Be("rare");
            result.Description.Should().Be("Shakespeare Translated Text");
        }
        private PokemonResponse GetValidPokemonResponse()
        {
            return new PokemonResponse()
            {
                name = "test",
                habitat = new PokemonHabitat() { name = "rare" },
                is_legendary = false,
                flavor_text_entries = new List<FlavorText>() { new FlavorText() { language = new Language() {name="en" }, flavor_text = "test description" } }
            };
        }

    }
}
