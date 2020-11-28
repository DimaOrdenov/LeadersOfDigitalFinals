using System;
using System.Threading.Tasks;
using Amazon.Comprehend.Model;

namespace LodFinals.Services
{
    public interface ILexService
    {
        Task<string> Conversation(string phrase);
        Task<string> DetectLang(string phrase);
        Task<string> GetIntent(string phrase);
        void SetUser(string userId);
    }
}
