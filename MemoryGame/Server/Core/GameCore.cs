using System;
using System.Threading;
using MemoryGame.Common;
using MemoryGame.Contracts;
using MemoryGame.Extensions;

namespace MemoryGame.Server.Core
{
    public class GameCore
    {
        private object _lock = new object();

        public event Action<string> TurnChanged;
        public event Action<SelectedCard> FirstCardSelected;
        public event Action<SelectedCard> SecondCardSelected;
        public event Action<SelectedCard, SelectedCard> SecondCardMatches;
        public event Action<SelectedCard, SelectedCard> SecondCardDoesntMatch;

        private readonly RoundRobin<string> _players;
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

            var cards = CardsFactory.Create(columns, rows);
            _cards = CardsShuffler.Shuffle(cards, 2);
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
            lock (_lock)
            {
                if (_cardSelectedIsBusy)
                {
                    return;
                }
                _cardSelectedIsBusy = true;
            }

            //only current player can select a card
            if (playertoken != _currentPlayer)
            {
                return;
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

            DetermineMatch(row, column, firstCardSelected, secondCardSelected);

            _isFirstCardSelected = false;

            NextTurn();

            _cardSelectedIsBusy = false;
        }

        private void DetermineMatch(int row, int column, Card firstCardSelected, Card secondCardSelected)
        {
            var firstCard = new SelectedCard
            {
                Row = _firstCardSelectedRow,
                Column = _firstCardSelectedColumn,
                ResourceIndex = _cards[_firstCardSelectedColumn, _firstCardSelectedRow].Number
            };

            var secondCard = new SelectedCard
            {
                Row = row,
                Column = column,
                ResourceIndex = _cards[column, row].Number
            };

            if (firstCardSelected.Number == secondCardSelected.Number)
            {
                //match
                SecondCardMatches.Raise(firstCard, secondCard);
            }
            else
            {
                //no match
                SecondCardDoesntMatch.Raise(firstCard, secondCard);
            }
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