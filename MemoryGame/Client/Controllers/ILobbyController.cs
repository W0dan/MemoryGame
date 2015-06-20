using System.Windows;

namespace MemoryGame.Client.Controllers
{
    public interface ILobbyController
    {
        UIElement Index(bool isHost);
    }
}