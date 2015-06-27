using System;
using System.Collections.Generic;
using MemoryGame.Contracts;

namespace MemoryGame.Client.Service
{
    public interface IPlayerContext
    {
        event Action<string> PlayerJoined;
        event Action<string, string> ChatMessageReceived;
        event Action<int, int> GameStarted;

        event Action YourTurn;
        event Action<string> PlayerIsOnTurn;
        event Action<SelectedCard> FirstCardSelected;
        event Action<SelectedCard> SecondCardSelected;
        event Action<SelectedCard, SelectedCard> SecondCardMatches;
        event Action<SelectedCard, SelectedCard> SecondCardDoesntMatch;
        event Action<string, int> PlayerReceivesPoints;
        event Action Victory;
        event Action Defeat;

        void SetJoinParameters(string host, string port, string playerName);
        void Join();
        void SendChatMessage(string text);
        List<string> GetPlayerList();
        string PlayerName { get; }
        void ReadyToRumble();
        void StartGame(string cardSet, int rows, int columns);
        void CardClicked(int row, int column);
    }
}