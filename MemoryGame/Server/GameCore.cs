using MemoryGame.Common;

namespace MemoryGame.Server
{
    public class GameCore
    {
        private readonly RoundRobin<string> _players;
        private string _currentPlayer;

        public GameCore(RoundRobin<string> players)
        {
            _players = players;
            _currentPlayer = _players.GetNext();
        }
    }
}