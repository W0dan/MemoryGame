using System.Windows;
using MemoryGame.Client.Navigation;
using MemoryGame.Client.Views;

namespace MemoryGame.Client.Controllers
{
    public class MainmenuController : IMainmenuController
    {
        private readonly INavigator _navigator;
        private readonly IMultiplayermenuController _multiplayermenuController;

        public MainmenuController(INavigator navigator, IMultiplayermenuController multiplayermenuController)
        {
            _navigator = navigator;
            _multiplayermenuController = multiplayermenuController;
        }

        public UIElement Index()
        {
            var mainmenu = new MainmenuControl();

            mainmenu.ExitButtonClicked += ExitButtonClicked;
            mainmenu.MultiplayerSelected += MultiplayerSelected;

            return mainmenu;
        }

        private void MultiplayerSelected()
        {
            _navigator.NavigateTo(() => _multiplayermenuController.Index());
        }

        private static void ExitButtonClicked()
        {
            Application.Current.Shutdown();
        }
    }
}