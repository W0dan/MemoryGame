using System.Windows;
using MemoryGame.Client.Navigation;
using MemoryGame.Client.Views;

namespace MemoryGame.Client.Controllers
{
    public class MultiplayermenuController : IMultiplayermenuController
    {
        private readonly INavigator _navigator;
        private readonly IHostmenuController _hostmenuController;

        public MultiplayermenuController(INavigator navigator, IHostmenuController hostmenuController)
        {
            _navigator = navigator;
            _hostmenuController = hostmenuController;
        }

        public UIElement Index()
        {
            var multiplayermenuControl = new MultiplayermenuControl();

            multiplayermenuControl.BackButtonClicked += BackButtonClicked;
            multiplayermenuControl.HostButtonClicked += HostButtonClicked;
            multiplayermenuControl.JoinButtonClicked += JoinButtonClicked;

            return multiplayermenuControl;
        }

        private void JoinButtonClicked()
        {

        }

        private void HostButtonClicked()
        {
            _navigator.NavigateTo(_hostmenuController.Index);
        }

        private void BackButtonClicked()
        {
            _navigator.NavigateFromHistory();
        }
    }
}