using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Google.Cloud.TextToSpeech.V1;
using LodFinals.DependencyServices;

namespace LodFinals.BusinessLayer
{
    public class GoogleCloudTextToSpeechLogic : IGoogleCloudTextToSpeechLogic
    {
        private readonly TextToSpeechClient _client;
        private readonly IPlatformFileManagerService _platformFileManagerService;

        public GoogleCloudTextToSpeechLogic(TextToSpeechClient client, IPlatformFileManagerService platformFileManagerService)
        {
            _client = client;
            _platformFileManagerService = platformFileManagerService;
        }

        public async Task<FileStream> TextToSpeechAsync(string text, CancellationToken token)
        {
            SynthesizeSpeechResponse response = await _client.SynthesizeSpeechAsync(
                new SynthesisInput { Text = text },
                new VoiceSelectionParams
                {
                    LanguageCode = "en-US",
                },
                new AudioConfig
                {
                    AudioEncoding = AudioEncoding.Mp3,
                },
                token);

            FileStream output = File.Create(Path.Combine(_platformFileManagerService.DownloadDirectory, "sample.mp3"));
            response.AudioContent.WriteTo(output);

            return output;
        }
    }
}
