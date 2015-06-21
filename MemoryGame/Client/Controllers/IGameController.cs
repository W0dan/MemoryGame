using System.Windows;

namespace MemoryGame.Client.Controllers
{
    public interface IGameController
    {
        UIElement Index(int rows, int columns);
    }
}