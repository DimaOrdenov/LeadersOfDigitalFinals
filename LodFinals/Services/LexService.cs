using System;
using System.Threading.Tasks;
using Amazon.Lex;
using Amazon.Lex.Model;

namespace LodFinals.Services
{
    public class LexService : ILexService
    {
        private readonly AmazonLexClient _client;
        private string _userId;

        public LexService(AmazonLexClient amazonLexClient)
        {
            _client = amazonLexClient;
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
            return (await Send(r)).IntentName;
        }

        private async Task<PostTextResponse> Send(PostTextRequest request) =>
            await _client.PostTextAsync(request);
    }
}
