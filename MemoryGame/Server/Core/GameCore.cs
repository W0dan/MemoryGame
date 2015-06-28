using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using MemoryGame.Common;
using MemoryGame.Contracts;
using MemoryGame.Extensions;

namespace MemoryGame.Server.Core
{
    public class GameCore
    {
        private readonly object _lock = new object();

        public event Action<Board> GameStarted;
        public event Action<string> TurnChanged;
        public event Action<SelectedCard> FirstCardSelected;
        public event Action<SelectedCard> SecondCardSelected;
        public event Action<SelectedCard, SelectedCard> SecondCardMatches;
        public event Action<SelectedCard, SelectedCard> SecondCardDoesntMatch;
        public event Action<string, int> PointsEarned;
        public event Action<string> GameEnd;

        private readonly RoundRobin<string> _players;
        private readonly string _cardSet;
        private readonly Dictionary<string, int> _points = new Dictionary<string, int>();
        private int _cardsLeft;

        private string _currentPlayer;
        private int _playersReady;

        private readonly Card[,] _cards;
        private int _firstCardSelectedRow;
        private int _firstCardSelectedColumn;
        private bool _isFirstCardSelected;

        private bool _cardSelectedIsBusy = false;
        private Board _board;

        public GameCore(RoundRobin<string> players, string cardSet, int numberOfCardsLevel)
        {
            _players = players;
            _cardSet = cardSet;

            _board = DetermineBoardLayout(numberOfCardsLevel);
            var rows = _board.Rows;
            var columns = _board.Columns;

            _cardsLeft = rows * columns;
            _cards = DeckOfCards.Deal(columns, rows);
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

                _cardsLeft -= 2;

                if (_cardsLeft <= 0)
                {
                    var maxPoints = _points.Max(p => p.Value);
                    var victoriousPlayers = _points.Where(p => p.Value == maxPoints);

                    var victoriousPlayer = victoriousPlayers.First().Key;
                    GameEnd.Raise(victoriousPlayer);
                }
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
            return SelectCard(row, column);
        }

        private SelectedCard GetFirstCardDetails()
        {
            return SelectCard(_firstCardSelectedRow, _firstCardSelectedColumn);
        }

        private Card SelectSecondCard(int row, int column)
        {
            var secondCardSelected = _cards[column, row];
            var selectedCard = SelectCard(row, column);

            SecondCardSelected.Raise(selectedCard);

            return secondCardSelected;
        }

        private void SelectFirstCard(int row, int column)
        {
            _isFirstCardSelected = true;

            _firstCardSelectedRow = row;
            _firstCardSelectedColumn = column;

            var selectedCard = SelectCard(row, column);

            FirstCardSelected.Raise(selectedCard);
        }

        private SelectedCard SelectCard(int row, int column)
        {
            var secondCard = new SelectedCard
            {
                Row = row,
                Column = column,
                ResourceIndex = _cards[column, row].Number,
                ResourceSet = _cardSet
            };
            return secondCard;
        }

        public void PlayerIsReady(string playertoken)
        {
            Interlocked.Increment(ref _playersReady);
            if (_playersReady == _players.Count)
            {
                NextTurn();
            }
        }

        public Board DetermineBoardLayout(int numberOfCardsLevel)
        {
            var rows=0;
            var columns=0;
            switch (numberOfCardsLevel)
            {
                case 1:
                case 2:
                case 3:
                case 5:
                case 7:
                    rows = numberOfCardsLevel + 1;
                    columns = numberOfCardsLevel + 2;
                    break;
                case 4:
                case 6:
                    rows = numberOfCardsLevel;
                    columns = numberOfCardsLevel + 2;
                    break;
                case 8:
                    rows = 7;
                    columns = 10;
                    break;
                case 9:
                    rows = 8;
                    columns = 9;
                    break;
            }

            return new Board(rows, columns);
        }

        public void Start()
        {
            GameStarted.Raise(_board);
        }
    }

    public class Board
    {
        public int Rows { get; private set; }
        public int Columns { get; private set; }

        public Board(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
        }
    }
}