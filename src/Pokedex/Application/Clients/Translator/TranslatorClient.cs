using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pokedex.Api.Application.Clients.Translator.Models;
using Pokedex.Api.Application.Exceptions;
using Pokedex.Api.Application.Options;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Pokedex.Api.Application.Clients.Translator
{
    public class TranslatorClient : ITranslatorClient
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly TranslatorServiceOptions _translatorServiceOptions;
        private readonly ILogger<TranslatorClient> _logger;

        public TranslatorClient(IHttpClientFactory clientFactory, IOptions<TranslatorServiceOptions> translatorServiceOptions, ILogger<TranslatorClient> logger)
        {
            _clientFactory = clientFactory;
            _translatorServiceOptions = translatorServiceOptions.Value;
            _logger = logger;
        }
      
        public async Task<string> GetTranslationAsync(string Text, TranslationType translationType)
        {
            _logger.LogInformation("Get @{translationType} Translation for {@Text}.", translationType, Text);
            var translationUrl = translationType == TranslationType.Yoda ? "yoda" :
            translationType == TranslationType.Shakespeare ? "shakespeare" :
            throw new DomainException("Translation requested without translation type");

            var client = _clientFactory.CreateClient();
            client.BaseAddress = _translatorServiceOptions.BaseUri;
            var response = await client.PostAsJsonAsync($"translate/{translationUrl}.json",new { text = Text });
            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                var translatedResponse = await JsonSerializer.DeserializeAsync
                    <Response>(responseStream);
                _logger.LogInformation("Pokemon retrieved : {@pokemon}", translatedResponse);
                return translatedResponse.contents.translated;
            }
            return Text;
        }
    }
}
