﻿using System;
using MemoryGame.Contracts;
using MemoryGame.Extensions;

namespace MemoryGame.Client.Service
{
    public class CallbackService : IPlayerCallback
    {
        public event Action<string, string> ChatMessageReceived;
        public event Action<string> PlayerJoined;
        public event Action<int, int> GameStarted;

        public void OnChatMessageReceived(string player, string message)
        {
            ChatMessageReceived.Raise(player, message);
        }

        public void OnPlayerJoined(string player)
        {
            PlayerJoined.Raise(player);
        }

        public void OnGameStarted(int rows, int columns)
        {
            GameStarted.Raise(rows, columns);
        }
    }
}