using System.Threading;
using System.Threading.Tasks;
using LodFinals.Definitions.Responses;
using NoTryCatch.BL.Core;
using NoTryCatch.Core.Services;
using RestSharp;

namespace LodFinals.BusinessLayer
{
    public class GoogleCloudTextToSpeechLogic : BaseLogic<GoogleCloudTextToSpeechResponse>, IGoogleCloudTextToSpeechLogic
    {
        public GoogleCloudTextToSpeechLogic(IRestClient client, UserContext context, IDebuggerService debuggerService)
            : base(client, context, debuggerService)
        {
        }

        protected override string Route => "https://texttospeech.googleapis.com/v1/text:synthesize";

        public Task<GoogleCloudTextToSpeechResponse> TextToSpeechAsync(string text, CancellationToken token)
        {
            //SynthesizeSpeechResponse response = await _client.SynthesizeSpeechAsync(
            //    new SynthesisInput { Text = text },
            //    new VoiceSelectionParams
            //    {
            //        LanguageCode = "en-US",
            //    },
            //    new AudioConfig
            //    {
            //        AudioEncoding = AudioEncoding.Mp3,
            //    },
            //    token);

            IRestRequest request = new RestRequest(Method.POST);
            request.AddJsonBody(new
            {
                input = new
                {
                    text,
                },
                voice = new
                {
                    languageCode = "en-US",
                },
                audioConfig = new
                {
                    audioEncoding = "MP3",
                }
            });

            //FileStream output = File.Create(Path.Combine(_platformFileManagerService.DownloadDirectory, "sample.mp3"));
            //response.AudioContent.WriteTo(output);

            return ExecuteAsync<GoogleCloudTextToSpeechResponse>(request, token);
        }
    }
}
