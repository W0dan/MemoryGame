using System;
using System.Collections.Generic;
using System.Threading;
using MemoryGame.Common;
using MemoryGame.Contracts;
using MemoryGame.Extensions;

namespace MemoryGame.Server.Core
{
    public class GameCore
    {
        private readonly object _lock = new object();

        public event Action<string> TurnChanged;
        public event Action<SelectedCard> FirstCardSelected;
        public event Action<SelectedCard> SecondCardSelected;
        public event Action<SelectedCard, SelectedCard> SecondCardMatches;
        public event Action<SelectedCard, SelectedCard> SecondCardDoesntMatch;
        public event Action<string, int> PointsEarned;

        private readonly RoundRobin<string> _players;
        private readonly Dictionary<string, int> _points = new Dictionary<string, int>();

        private string _currentPlayer;
        private int _playersReady;

        private readonly Card[,] _cards;
        private int _firstCardSelectedRow;
        private int _firstCardSelectedColumn;
        private bool _isFirstCardSelected;

        private bool _cardSelectedIsBusy = false;

        public GameCore(RoundRobin<string> players, int rows, int columns)
        {
            _players = players;

            _cards = CardsFactory.Create(columns, rows);
        }

        public void Start()
        {
            NextTurn();
        }

        private void NextTurn()
        {
            _firstCardSelectedRow = 0;
            _firstCardSelectedColumn = 0;

            _currentPlayer = _players.GetNext();
            TurnChanged.Raise(_currentPlayer);
        }

        //todo: this method needs some real testing !!!
        public void CardSelected(string playertoken, int row, int column)
        {
            //only current player can select a card
            if (playertoken != _currentPlayer)
            {
                return;
            }

            lock (_lock)
            {
                if (_cardSelectedIsBusy)
                {
                    return;
                }
                _cardSelectedIsBusy = true;
            }

            if (!_isFirstCardSelected)
            {
                SelectFirstCard(row, column);
                _cardSelectedIsBusy = false;
                return;
            }

            if (_firstCardSelectedRow == row && _firstCardSelectedColumn == column)
            {
                _cardSelectedIsBusy = false;
                return;
            }

            var firstCardSelected = _cards[_firstCardSelectedColumn, _firstCardSelectedRow];
            var secondCardSelected = SelectSecondCard(row, column);

            //wait 3 seconds before removing/resetting cards
            Thread.Sleep(3000);

            var firstCardDetails = GetFirstCardDetails();
            var secondCardDetails = GetSecondCardDetails(row, column);

            if (IsMatch(firstCardSelected, secondCardSelected))
            {
                SecondCardMatches.Raise(firstCardDetails, secondCardDetails);
                AddPoint(playertoken);
                PointsEarned.Raise(playertoken, _points[playertoken]);
            }
            else
            {
                SecondCardDoesntMatch.Raise(firstCardDetails, secondCardDetails);
                NextTurn();
            }

            _isFirstCardSelected = false;
            
            _cardSelectedIsBusy = false;
        }

        private void AddPoint(string playertoken)
        {
            if (!_points.ContainsKey(playertoken))
            {
                _points.Add(playertoken, 1);
            }
            else
            {
                _points[playertoken]++;
            }
        }

        private static bool IsMatch(Card firstCardSelected, Card secondCardSelected)
        {
            return firstCardSelected.Number == secondCardSelected.Number;
        }

        private SelectedCard GetSecondCardDetails(int row, int column)
        {
            var secondCard = new SelectedCard
            {
                Row = row,
                Column = column,
                ResourceIndex = _cards[column, row].Number
            };
            return secondCard;
        }

        private SelectedCard GetFirstCardDetails()
        {
            var firstCard = new SelectedCard
            {
                Row = _firstCardSelectedRow,
                Column = _firstCardSelectedColumn,
                ResourceIndex = _cards[_firstCardSelectedColumn, _firstCardSelectedRow].Number
            };
            return firstCard;
        }

        private Card SelectSecondCard(int row, int column)
        {
            var secondCardSelected = _cards[column, row];

            SecondCardSelected.Raise(new SelectedCard
            {
                Row = row,
                Column = column,
                ResourceIndex = _cards[column, row].Number
            });
            return secondCardSelected;
        }

        private void SelectFirstCard(int row, int column)
        {
            _firstCardSelectedRow = row;
            _firstCardSelectedColumn = column;

            _isFirstCardSelected = true;

            FirstCardSelected.Raise(new SelectedCard
            {
                Row = row,
                Column = column,
                ResourceIndex = _cards[column, row].Number
            });
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