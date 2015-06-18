using System;
using MemoryGame.Contracts;
using MemoryGame.Extensions;

namespace MemoryGame.Client.Service
{
    public class CallbackService : IPlayerCallback
    {
        public event Action<string, string> ChatMessageReceived;

        public void OnChatMessageReceived(string player, string message)
        {
            ChatMessageReceived.Raise(player, message);
        }
    }
}