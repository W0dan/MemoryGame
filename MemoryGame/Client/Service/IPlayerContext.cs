using System;
using System.Collections.Generic;

namespace MemoryGame.Client.Service
{
    public interface IPlayerContext
    {
        event Action<string> PlayerJoined;
        event Action<string, string> ChatMessageReceived;
        event Action<int, int> GameStarted;

        void SetJoinParameters(string host, string port, string playerName);
        void Join();
        void SendChatMessage(string text);
        List<string> GetPlayerList();
        string PlayerName { get; }
        void StartGame(int rows, int columns);
    }
}