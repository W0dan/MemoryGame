using System.Collections.Generic;
using System.ServiceModel;
using MemoryGame.Contracts;

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

        public void StartGame(string playertokenFrom, int rows, int columns)
        {
            //create a roundrobin collection of the players
            var players = _players.GetPlayersRoundRobin();
            _game = new GameCore(players, rows, columns);

            _game.TurnChanged += TurnChanged;

            _players.Send(callback => callback.OnGameStarted(rows, columns));

            //_game.Start();

            //todo: send to starting player that it's his/her turn
        }

        private void TurnChanged(string playertoken)
        {
            _players.Send(playertoken, (player, callback) => callback.OnPlayerIsOnTurn(player.Name));
            _players.SendTo(playertoken, callback => callback.OnYourTurn());
        }

        public void CardClicked(string playertoken, int row, int column)
        {
            _game.CardClicked(playertoken, row, column);

            //todo: send result of cardclicked to players
            //could be: - show card c (x,y) being resource nr z
            //          - remove card 1 (x1,y1), 2 (x2,y2)
            //          - cover card 1 (x1,y1), 2 (x2,y2)
        }
    }
}
