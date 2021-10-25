using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pokedex.Api.Application.Clients.Translator.Models;
using Pokedex.Api.Application.Exceptions;
using Pokedex.Api.Application.Options;
using System;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace Pokedex.Api.Application.Clients.Translator
{
    public class TranslatorClient : ITranslatorClient
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly TranslatorServiceOptions _translatorServiceOptions;
        private readonly ILogger<TranslatorClient> _logger;
        private const string TranslationQueryParmName = "text";


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
            string url = GetRequestURL(Text, translationUrl, _translatorServiceOptions.BaseUri.AbsoluteUri);
            var response = await client.GetAsync(url);

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

        private static string GetRequestURL(string Text, string translationUrl, string baseUri)
        {
            var builder = new UriBuilder($"{baseUri}translate/{translationUrl}.json?text={HttpUtility.UrlPathEncode(Text)}");
            var query = HttpUtility.ParseQueryString(builder.Query);
            query[TranslationQueryParmName] = Uri.EscapeUriString(Text);
            builder.Query = query.ToString();
            return  builder.ToString();
        }
    }
}
