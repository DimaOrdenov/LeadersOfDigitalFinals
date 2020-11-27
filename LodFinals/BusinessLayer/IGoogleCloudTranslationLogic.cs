using System;
using System.Threading;
using System.Threading.Tasks;

namespace LodFinals.BusinessLayer
{
    public interface IGoogleCloudTranslationLogic
    {
        Task<string> TranslateTextAsync(string text, CancellationToken cancellationToken, string targetLanguage = "ru");
    }
}
