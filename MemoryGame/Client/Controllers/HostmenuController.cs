using System.Windows;
using MemoryGame.Client.Views;

namespace MemoryGame.Client.Controllers
{
    public class HostmenuController : IHostmenuController
    {
        public UIElement Index()
        {
            return new HostmenuControl();
        }
    }
}