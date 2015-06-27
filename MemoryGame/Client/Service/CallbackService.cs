using System;
using System.ServiceModel;
using MemoryGame.Contracts;
using MemoryGame.Extensions;

namespace MemoryGame.Client.Service
{
    [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
    public class CallbackService : IPlayerCallback
    {
        public event Action<string, string> ChatMessageReceived;
        public event Action<string> PlayerJoined;
        public event Action<int, int> GameStarted;

        public event Action YourTurn;
        public event Action<string> PlayerIsOnTurn;
        public event Action<SelectedCard> FirstCardSelected;
        public event Action<SelectedCard> SecondCardSelected;
        public event Action<SelectedCard, SelectedCard> SecondCardMatches;
        public event Action<SelectedCard, SelectedCard> SecondCardDoesntMatch;
        public event Action<string, int> PlayerReceivesPoints;
        public event Action<string> GameEnd;

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

        public void OnYourTurn()
        {
            YourTurn.Raise();
        }

        public void OnPlayerIsOnTurn(string player)
        {
            PlayerIsOnTurn.Raise(player);
        }

        public void OnFirstCardSelected(SelectedCard card)
        {
            FirstCardSelected.Raise(card);
        }

        public void OnSecondCardSelected(SelectedCard card)
        {
            SecondCardSelected.Raise(card);
        }

        public void OnSecondCardMatches(SelectedCard firstCard, SelectedCard secondCard)
        {
            SecondCardMatches.Raise(firstCard, secondCard);
        }

        public void OnSecondCardDoesntMatch(SelectedCard firstCard, SelectedCard secondCard)
        {
            SecondCardDoesntMatch.Raise(firstCard, secondCard);
        }

        public void OnPlayerReceivesPoints(string player, int points)
        {
            PlayerReceivesPoints.Raise(player, points);
        }

        public void OnGameEnd(string victoriousPlayer)
        {
            GameEnd.Raise(victoriousPlayer);
        }
    }
}