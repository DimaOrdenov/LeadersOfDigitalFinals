using System.Threading.Tasks;

namespace LodFinals.DependencyServices
{
    public interface IPlatformAudioPlayerService
    {
        Task Play(string filePath);
    }
}
