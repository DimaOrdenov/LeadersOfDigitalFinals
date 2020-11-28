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
        private readonly ExtendedUserContext _context;

        public GoogleCloudTextToSpeechLogic(IRestClient client, ExtendedUserContext context, IDebuggerService debuggerService)
            : base(client, context, debuggerService)
        {
            _context = context;
        }

        protected override string Route => "https://texttospeech.googleapis.com/v1/text:synthesize";

        public Task<GoogleCloudTextToSpeechResponse> TextToSpeechAsync(string text, CancellationToken token)
        {
            IRestRequest request = new RestRequest(Method.POST);
            request.AddJsonBody(new
            {
                input = new
                {
                    text,
                },
                voice = new
                {
                    languageCode = _context.SettingAccent,
                },
                audioConfig = new
                {
                    audioEncoding = "MP3",
                }
            });

            return ExecuteAsync<GoogleCloudTextToSpeechResponse>(request, token);
        }
    }
}
