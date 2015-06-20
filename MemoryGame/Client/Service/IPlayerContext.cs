﻿using System;
using System.Collections.Generic;

namespace MemoryGame.Client.Service
{
    public interface IPlayerContext
    {
        event Action<string> PlayerJoined;
        event Action<string, string> ChatMessageReceived;

        void Join(string host, string port, string playerName);
        void SendChatMessage(string text);
        List<string> GetPlayerList();
        string PlayerName { get; }
    }
}