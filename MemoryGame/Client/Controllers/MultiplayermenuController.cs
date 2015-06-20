using System.Windows;
using MemoryGame.Client.Navigation;
using MemoryGame.Client.Views;

namespace MemoryGame.Client.Controllers
{
    public class MultiplayermenuController : IMultiplayermenuController
    {
        private readonly INavigator _navigator;
        private readonly IHostmenuController _hostmenuController;
        private readonly IJoinController _joinController;

        public MultiplayermenuController(INavigator navigator, IHostmenuController hostmenuController, IJoinController joinController)
        {
            _navigator = navigator;
            _hostmenuController = hostmenuController;
            _joinController = joinController;
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
            _navigator.NavigateTo(_joinController.Index);
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