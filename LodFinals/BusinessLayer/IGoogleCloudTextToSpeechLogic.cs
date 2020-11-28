using System.Threading;
using System.Threading.Tasks;
using LodFinals.Definitions.Responses;

namespace LodFinals.BusinessLayer
{
    public interface IGoogleCloudTextToSpeechLogic
    {
        Task<GoogleCloudTextToSpeechResponse> TextToSpeechAsync(string text, CancellationToken token);
    }
}
