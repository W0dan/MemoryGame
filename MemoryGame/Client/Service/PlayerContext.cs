using System;
using System.Collections.Generic;
using MemoryGame.Contracts;
using MemoryGame.Extensions;

namespace MemoryGame.Client.Service
{
    public class PlayerContext : IPlayerContext
    {
        public event Action<string> PlayerJoined;
        public event Action<string, string> ChatMessageReceived;
        public event Action<int, int> GameStarted;

        public event Action YourTurn;
        public event Action<string> PlayerIsOnTurn;
        public event Action<SelectedCard> FirstCardSelected;
        public event Action<SelectedCard> SecondCardSelected;
        public event Action<SelectedCard, SelectedCard> SecondCardMatches;
        public event Action<SelectedCard, SelectedCard> SecondCardDoesntMatch;
        public event Action<string, int> PlayerReceivesPoints;

        private MultiplayerProxy _service;
        private string _playerToken;
        private string _host;
        private string _port;

        public string PlayerName { get; private set; }

        public void SetJoinParameters(string host, string port, string playerName)
        {
            PlayerName = playerName;

            _host = host;
            _port = port;
        }

        public void Join()
        {
            _service = new MultiplayerProxy(_host, _port);

            _service.ChatMessageReceived += ChatMessageReceived;
            _service.PlayerJoined += PlayerJoined;
            _service.GameStarted += GameStarted;

            _service.YourTurn += () => YourTurn.Raise();
            _service.PlayerIsOnTurn += player => PlayerIsOnTurn.Raise(player);
            _service.FirstCardSelected += card => FirstCardSelected.Raise(card);
            _service.SecondCardSelected += card => SecondCardSelected.Raise(card);
            _service.SecondCardMatches += (firstCard, secondCard) => SecondCardMatches.Raise(firstCard, secondCard);
            _service.SecondCardDoesntMatch += (firstCard, secondCard) => SecondCardDoesntMatch.Raise(firstCard, secondCard);
            _service.PlayerReceivesPoints += (player, points) => PlayerReceivesPoints.Raise(player, points);

            //todo: display error when join fails with exception
            //todo: display error when join returns null token (meaning playerName allready exists on the server)
            _playerToken = _service.Join(PlayerName);
        }

        public void SendChatMessage(string text)
        {
            _service.SendChatMessage(_playerToken, text);
        }

        public List<string> GetPlayerList()
        {
            return _service.GetPlayerList(_playerToken);
        }

        public void ReadyToRumble()
        {
            _service.ReadyToRumble(_playerToken);
        }

        public void StartGame(int rows, int columns)
        {
            _service.StartGame(_playerToken, rows, columns);
        }

        public void CardClicked(int row, int column)
        {
            _service.CardClicked(_playerToken, row, column);
        }
    }
}