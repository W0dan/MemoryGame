using System;
using System.Collections.Generic;

namespace MemoryGame.Client.Service
{
    public interface IPlayerContext
    {
        event Action<string> PlayerJoined;
        event Action<string, string> ChatMessageReceived;

        void SetJoinParameters(string host, string port, string playerName);
        void Join();
        void SendChatMessage(string text);
        List<string> GetPlayerList();
        string PlayerName { get; }
    }
}