using System.Windows;
using MemoryGame.Client.Views;

namespace MemoryGame.Client.Controllers
{
    public class GameController : IGameController
    {
        public GameController()
        {
            
        }

        public UIElement Index(int rows, int columns)
        {
            var gameControl = new GameControl();

            return gameControl;
        }
    }
}