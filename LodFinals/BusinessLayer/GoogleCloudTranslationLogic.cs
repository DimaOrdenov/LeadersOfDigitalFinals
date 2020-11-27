using System;
using System.Threading;
using System.Threading.Tasks;
using Google.Cloud.Translation.V2;

namespace LodFinals.BusinessLayer
{
    public class GoogleCloudTranslationLogic : IGoogleCloudTranslationLogic
    {
        private readonly TranslationClient _client;

        public GoogleCloudTranslationLogic(TranslationClient client)
        {
            _client = client;
        }

        public async Task<string> TranslateTextAsync(string text, CancellationToken cancellationToken, string targetLanguage = "ru") =>
            (await _client.TranslateTextAsync(text, targetLanguage, cancellationToken: cancellationToken)).TranslatedText;
    }
}
