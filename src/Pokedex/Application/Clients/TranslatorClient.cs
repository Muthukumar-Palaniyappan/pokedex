using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pokedex.Api.Application.Options;
using System;
using System.Net.Http;

namespace Pokedex.Api.Application.Clients
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

        public string GetShakespeareTranslation(string Text)
        {
            _logger.LogInformation("Get ShakespeareTranslation for {@Text}.", Text);
            var client = _clientFactory.CreateClient();
            client.BaseAddress = _translatorServiceOptions.BaseUri;
            var response = client.GetAsync($"/api/shakespeare/{Text}").Result;
            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsStringAsync().Result;
            }
            return Text;
        }

        public string GetYodaTranslation(string Text)
        {
            _logger.LogInformation("Get YodaTranslation for {@Text}.", Text);
            var client = _clientFactory.CreateClient();
            client.BaseAddress = _translatorServiceOptions.BaseUri;
            var response = client.GetAsync($"/api/yoda/{Text}").Result;
            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsStringAsync().Result;
            }
            return Text;
        }
    }
}
