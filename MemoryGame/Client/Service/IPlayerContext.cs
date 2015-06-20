using System;

namespace MemoryGame.Client.Service
{
    public interface IPlayerContext
    {
        void Join(string host, string port, string playerName);
        event Action<string, string> ChatMessageReceived;
        void SendChatMessage(string text);
        string PlayerName { get; }
    }
}