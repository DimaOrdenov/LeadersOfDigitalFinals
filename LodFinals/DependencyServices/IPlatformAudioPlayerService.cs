using System;
using System.Threading.Tasks;

namespace LodFinals.DependencyServices
{
    public interface IPlatformAudioPlayerService
    {
        event EventHandler Finished;

        Task PlayAsync(string filePath);

        void Stop();
    }
}
