using System.Collections.Generic;
using System.ServiceModel;
using MemoryGame.Contracts;
using MemoryGame.Server.Core;

namespace MemoryGame.Server
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Reentrant, UseSynchronizationContext = false)]
    public class MultiplayerService : IMultiplayerService
    {
        private readonly SubscriberCollection<IPlayerCallback> _players = new SubscriberCollection<IPlayerCallback>();
        private GameCore _game;

        public string Join(string playerName)
        {
            var joinedPlayer = _players.Add(playerName, OperationContext.Current.GetCallbackChannel<IPlayerCallback>());

            if (joinedPlayer == null)
            {
                return null;
            }

            _players.Send(joinedPlayer.Token, (player, callback) => callback.OnPlayerJoined(player.Name));
            _players.Send(joinedPlayer.Token, (player, callback) => callback.OnChatMessageReceived(player.Name, string.Format("{0} has joined", joinedPlayer.Name)));

            return joinedPlayer.Token;
        }

        public void SendChatMessage(string playertoken, string message)
        {
            _players.Send(playertoken, (player, callback) => callback.OnChatMessageReceived(player.Name, message));
        }

        public List<string> GetPlayerList(string playertoken)
        {
            return _players.GetPlayerNames();
        }

        public void ReadyToRumble(string playertoken)
        {
            _game.PlayerIsReady(playertoken);
        }

        public void StartGame(string playertokenFrom, string cardSet, int rows, int columns)
        {
            //create a roundrobin collection of the players
            var players = _players.GetPlayersRoundRobin();
            _game = new GameCore(players, cardSet, rows, columns);

            _game.TurnChanged += TurnChanged;
            _game.FirstCardSelected += FirstCardSelected;
            _game.SecondCardSelected += SecondCardSelected;
            _game.SecondCardMatches += SecondCardMatches;
            _game.SecondCardDoesntMatch += SecondCardDoesntMatch;
            _game.PointsEarned += PointsEarned;
            _game.GameEnd += GameEnd;

            _players.Send(callback => callback.OnGameStarted(rows, columns));
        }

        private void GameEnd(string playertoken)
        {
            _players.Send(playertoken, (player, callback) => callback.OnGameEnd(player.Name));
        }

        private void PointsEarned(string playertoken, int points)
        {
            _players.Send(playertoken, (player, callback) => callback.OnPlayerReceivesPoints(player.Name, points));
        }

        private void TurnChanged(string playertoken)
        {
            _players.Send(playertoken, (player, callback) => callback.OnPlayerIsOnTurn(player.Name));
            _players.SendTo(playertoken, callback => callback.OnYourTurn());
        }

        public void CardClicked(string playertoken, int row, int column)
        {
            _game.CardSelected(playertoken, row, column);
        }

        private void FirstCardSelected(SelectedCard card)
        {
            _players.Send(callback => callback.OnFirstCardSelected(card));
        }

        private void SecondCardSelected(SelectedCard card)
        {
            _players.Send(callback => callback.OnSecondCardSelected(card));
        }

        private void SecondCardMatches(SelectedCard firstCard, SelectedCard secondCard)
        {
            _players.Send(callback => callback.OnSecondCardMatches(firstCard, secondCard));
        }

        private void SecondCardDoesntMatch(SelectedCard firstCard, SelectedCard secondCard)
        {
            _players.Send(callback => callback.OnSecondCardDoesntMatch(firstCard, secondCard));
        }
    }
}
