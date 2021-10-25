using System.Threading.Tasks;

namespace Pokedex.Api.Application.Clients.Translator
{
    public interface ITranslatorClient
    {
        public Task<string> GetTranslationAsync(string Text, TranslationType translationType);
    }
}
