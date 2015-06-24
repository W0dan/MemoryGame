using System;
using System.Threading;
using MemoryGame.Common;
using MemoryGame.Extensions;

namespace MemoryGame.Server
{
    public class GameCore
    {
        public event Action<string> TurnChanged;

        private readonly RoundRobin<string> _players;
        private string _currentPlayer;
        private int _playersReady;

        public GameCore(RoundRobin<string> players, int rows, int columns)
        {
            _players = players;
            _currentPlayer = _players.GetNext();
        }

        public void Start()
        {
            TurnChanged.Raise(_currentPlayer);
        }

        public void CardClicked(string playertoken, int row, int column)
        {
            if (playertoken != _currentPlayer)
            {
                return;
            }


        }

        public void PlayerIsReady(string playertoken)
        {
            Interlocked.Increment(ref _playersReady);
            if (_playersReady == _players.Count)
            {
                Start();
            }
        }
    }
}