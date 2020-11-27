using System;
namespace LodFinals.DependencyServices
{
    public interface IPlatformSpeechToTextService
    {
        event EventHandler<string> SpeechRecognitionFinished;

        void StartSpeechToText();
    }
}
