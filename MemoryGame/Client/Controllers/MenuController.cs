using System.Windows;
using MemoryGame.Client.Views;

namespace MemoryGame.Client.Controllers
{
    public class MenuController : IMenuController
    {
        public UIElement Index()
        {
            var mainmenu = new MainmenuControl();

            mainmenu.ExitButtonClicked+=ExitButtonClicked;

            return mainmenu;
        }

        private static void ExitButtonClicked()
        {
            Application.Current.Shutdown();
        }
    }
}