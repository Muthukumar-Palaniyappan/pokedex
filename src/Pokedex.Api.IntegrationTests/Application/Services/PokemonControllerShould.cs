using FluentAssertions;
using System.Net;
using System.Threading.Tasks;
using Xunit;
using WireMock;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using AutoFixture;
using Pokedex.Api.Application.Clients.PokeApi.Models;
using TransModel = Pokedex.Api.Application.Clients.Translator.Models;
using System.Net.Http.Formatting;
using System.Net.Http;
using Pokedex.Contract;
using System.Collections.Generic;

namespace Pokedex.Api.IntegrationTests.Application.Services
{
    public class PokemonControllerShould: IntegrationTest
    {
        private readonly Fixture _fixture;
        public PokemonControllerShould()
        {
            _fixture = new Fixture();
        }


        [Fact]
        public async Task GetPokemon_Returns_Pokemon()
        {
            var pokemonName = _fixture.Create<string>();
            var pokemonResponse =  _fixture.Build<PokemonResponse>()
                .With(pokemon => pokemon.name, pokemonName)
                .Create();

            WireMockServers.mockPokedexServer
                .Given(Request.Create().WithPath($"/api/v2/pokemon-species/{pokemonName}"))
                .RespondWith(Response.Create().WithBodyAsJson(pokemonResponse));

             var response = await _testClient.GetAsync($"Pokemon/{pokemonName}");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var actualPokemon = await response.Content.ReadAsAsync<Pokemon>();
            actualPokemon.Name.Should().Be(pokemonName);

        }

        [Fact]
        public async Task GetPokemonTranslated_Returns_YodaTranslatedPokemon()
        {
            var pokemonName = _fixture.Create<string>();
            var description = _fixture.Create<string>();
            var translated = _fixture.Create<string>();
            var pokemonResponse = _fixture.Build<PokemonResponse>()
                .With(pokemon => pokemon.name, pokemonName)
                .With(pokemon => pokemon.is_legendary, true)
                .With(pokemon => pokemon.flavor_text_entries,
                new List<FlavorText>() { new FlavorText() { flavor_text = description, language = new Language() { name = "en" } } })
                .Create();

            WireMockServers.mockPokedexServer
                .Given(Request.Create().WithPath($"/api/v2/pokemon-species/{pokemonName}"))
                .RespondWith(Response.Create().WithBodyAsJson(pokemonResponse));
            var content = _fixture.Build<TransModel.Contents>()
                .With(content => content.translated, translated).Create();
            var translatedResponse = _fixture.Build<TransModel.Response>()
                .With(trans => trans.contents, content)
                .Create();
            WireMockServers.mockTranslatorServer
                .Given(Request.Create().WithPath($"/translate/yoda.json").UsingPost())
                .RespondWith(Response.Create().WithBodyAsJson(translatedResponse));


            var response = await _testClient.GetAsync($"Pokemon/translated/{pokemonName}");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var actualPokemon = await response.Content.ReadAsAsync<Pokemon>();
            actualPokemon.Name.Should().Be(pokemonName);
            actualPokemon.Description.Should().Be(translated);

        }
    }
}
