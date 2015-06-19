using System.Windows;
using MemoryGame.Client.Navigation;
using MemoryGame.Client.Views;

namespace MemoryGame.Client.Controllers
{
    public class MultiplayermenuController : IMultiplayermenuController
    {
        private readonly INavigator _navigator;

        public MultiplayermenuController(INavigator navigator)
        {
            _navigator = navigator;
        }

        public UIElement Index()
        {
            var multiplayermenuControl = new MultiplayermenuControl();

            multiplayermenuControl.BackButtonClicked += BackButtonClicked;

            return multiplayermenuControl;
        }

        private void BackButtonClicked()
        {
            _navigator.NavigateFromHistory();
        }
    }
}