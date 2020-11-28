using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace LodFinals.BusinessLayer
{
    public interface IGoogleCloudTextToSpeechLogic
    {
        Task<FileStream> TextToSpeechAsync(string text, CancellationToken token);
    }
}
