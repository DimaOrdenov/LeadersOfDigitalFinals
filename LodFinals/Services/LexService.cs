using System;
using System.Linq;
using System.Threading.Tasks;
using Amazon.Comprehend;
using Amazon.Comprehend.Model;
using Amazon.Lex;
using Amazon.Lex.Model;

namespace LodFinals.Services
{
    public class LexService : ILexService
    {
        private readonly AmazonLexClient _lexClient;
        private readonly AmazonComprehendClient _amazonComprehendClient;
        private string _userId;

        public LexService(AmazonLexClient amazonLexClient, AmazonComprehendClient amazonComprehendClient)
        {
            _lexClient = amazonLexClient;
            _amazonComprehendClient = amazonComprehendClient;
        }

        public void SetUser(string userId)
        {
            _userId = userId;
        }

        public async Task<string> GetIntent(string phrase)
        {
            var r = new PostTextRequest() { BotName = "LodFinals", UserId = _userId, InputText = phrase, BotAlias = "lod_intent" };
            return (await Send(r)).IntentName;
        }

        public async Task<string> Conversation(string phrase)
        {
            var r = new PostTextRequest() { BotName = "LodConversation", UserId = _userId, InputText = phrase, BotAlias = "lod_conversation" };
            return (await Send(r)).Message;
        }

        public async Task<string> DetectLang(string phrase)
        {
            var r = await _amazonComprehendClient.DetectDominantLanguageAsync(new DetectDominantLanguageRequest() { Text = phrase });
            return r.Languages.FirstOrDefault()?.LanguageCode;
        }

        private async Task<PostTextResponse> Send(PostTextRequest request) =>
            await _lexClient.PostTextAsync(request);
    }
}
