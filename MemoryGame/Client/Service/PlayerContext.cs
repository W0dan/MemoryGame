﻿using System;
using System.Collections.Generic;

namespace MemoryGame.Client.Service
{
    public class PlayerContext : IPlayerContext
    {
        public event Action<string> PlayerJoined;
        public event Action<string, string> ChatMessageReceived;
        public event Action<int, int> GameStarted;

        private MultiplayerProxy _service;
        private string _playerToken;

        public string PlayerName { get; private set; }

        public void Join(string host, string port, string playerName)
        {
            PlayerName = playerName;

            _service = new MultiplayerProxy(host, port);

            _service.ChatMessageReceived += ChatMessageReceived;
            _service.PlayerJoined += PlayerJoined;
            _service.GameStarted += GameStarted;

            //todo: display error when join fails with exception
            //todo: display error when join returns null token (meaning playerName allready exists on the server)
            _playerToken = _service.Join(playerName);
        }

        public void SendChatMessage(string text)
        {
            _service.SendChatMessage(_playerToken, text);
        }

        public List<string> GetPlayerList()
        {
            return _service.GetPlayerList(_playerToken);
        }
        public void StartGame(int rows, int columns)
        {
            _service.StartGame(_playerToken, rows, columns);
        }
    }
}