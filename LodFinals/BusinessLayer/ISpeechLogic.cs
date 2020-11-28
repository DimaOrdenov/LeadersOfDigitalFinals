using System.Threading;
using System.Threading.Tasks;

namespace LodFinals.BusinessLayer
{
    public interface ISpeechLogic
    {
        Task<string> ConvertTextToSpeechAsync(string text, string languageCode, CancellationToken token);
    }
}
