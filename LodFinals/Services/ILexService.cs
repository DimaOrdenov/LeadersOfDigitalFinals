using System;
using System.Threading.Tasks;

namespace LodFinals.Services
{
    public interface ILexService
    {
        Task<string> Conversation(string phrase);
        Task<string> GetIntent(string phrase);
        void SetUser(string userId);
    }
}
