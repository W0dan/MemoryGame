using System.Windows;
using MemoryGame.Client.Views;

namespace MemoryGame.Client.Controllers
{
    public class CreateMultiplayerGameController : ICreateMultiplayerGameController
    {
        public UIElement Index()
        {
            return new CreateMultiplayerGameControl();
        }
    }
}