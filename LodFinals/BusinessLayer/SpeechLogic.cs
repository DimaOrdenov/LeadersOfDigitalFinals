using System;
using System.Threading;
using System.Threading.Tasks;
using NoTryCatch.BL.Core;
using NoTryCatch.Core.Services;
using RestSharp;

namespace LodFinals.BusinessLayer
{
    public class SpeechLogic : BaseLogic<string>, ISpeechLogic
    {
        public SpeechLogic(IRestClient client, UserContext context, IDebuggerService debuggerService)
            : base(client, context, debuggerService)
        {
        }

        protected override string Route => "speech";

        public Task<string> ConvertTextToSpeechAsync(string text, string languageCode, CancellationToken token)
        {
            IRestRequest request = new RestRequest(Route, Method.GET);
            request.AddParameter("text", text);
            request.AddParameter("language", languageCode);

            return ExecuteAsync<string>(request, token);
        }
    }
}
