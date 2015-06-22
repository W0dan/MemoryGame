using System;
using System.ServiceModel;
using MemoryGame.Contracts;
using MemoryGame.Extensions;

namespace MemoryGame.Client.Service
{
    [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
    public class CallbackService : IPlayerCallback
    {
        public event Action<string, string> ChatMessageReceived;
        public event Action<string> PlayerJoined;

        public void OnChatMessageReceived(string player, string message)
        {
            ChatMessageReceived.Raise(player, message);
        }

        public void OnPlayerJoined(string player)
        {
            PlayerJoined.Raise(player);
        }
    }
}