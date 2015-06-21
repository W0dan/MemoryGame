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

            _players.Send(joinedPlayer.Token, (player, callback) => callback.OnPlayerJoined(player));
            _players.Send(joinedPlayer.Token, (player, callback) => callback.OnChatMessageReceived(player, string.Format("{0} has joined", joinedPlayer.Name)));

            return joinedPlayer.Token;
        }

        public void SendChatMessage(string playertoken, string message)
        {
            _players.Send(playertoken, (player, callback) => callback.OnChatMessageReceived(player, message));
        }

        public List<string> GetPlayerList(string playertoken)
        {
            return _players.GetPlayerNames();
        }

        public void StartGame(string playertokenFrom, int rows, int columns)
        {
            //create a roundrobin collection of the players
            var players = _players.GetPlayersRoundRobin();
            _game = new GameCore(players);
            //todo: send to players that the game is started
            _players.Send(playertokenFrom, (player, callback)=> callback.OnGameStarted(rows, columns));

            //todo: send to starting player that it's his/her turn
        }
    }
}
