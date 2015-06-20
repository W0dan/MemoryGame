using System;

namespace MemoryGame.Client.Service
{
    public class PlayerContext : IPlayerContext
    {
        public event Action<string, string> ChatMessageReceived;

        private MultiplayerProxy _service;
        private string _playerToken;

        public void Join(string host, string port, string playerName)
        {
            PlayerName = playerName;

            _service = new MultiplayerProxy(host, port);

            _service.ChatMessageReceived += ChatMessageReceived;

            //todo: display error when join fails with exception
            //todo: display error when join returns null token (meaning playerName allready exists on the server)
            _playerToken = _service.Join(playerName);
        }

        public void SendChatMessage(string text)
        {
            _service.SendChatMessage(_playerToken, text);
        }

        public string PlayerName { get; private set; }
    }
}