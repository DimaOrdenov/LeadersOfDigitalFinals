using System;
namespace LodFinals.DependencyServices
{
    public interface IPlatformSpeechToTextService
    {
        event EventHandler<string> ErrorOccured;

        event EventHandler<string> Finished;

        event EventHandler<string> PartialResultsReceived;

        void StartSpeechToText();

        void StopSpeechToText();
    }
}
