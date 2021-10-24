using Pokedex.Api.Application.Clients.Models;

namespace Pokedex.Api.Application.Clients
{
    public interface ITranslatorClient
    {
        public string GetYodaTranslation(string Text);
        public string GetShakespeareTranslation(string Text);
    }
}
