using System.Windows;
using MemoryGame.Client.Navigation;
using MemoryGame.Client.Service;
using MemoryGame.Client.Views;

namespace MemoryGame.Client.Controllers
{
    public class JoinController : IJoinController
    {
        private readonly INavigator _navigator;
        private readonly IPlayerContext _playerContext;
        private readonly ILobbyController _lobbyController;

        public JoinController(INavigator navigator, IPlayerContext playerContext, ILobbyController lobbyController)
        {
            _navigator = navigator;
            _playerContext = playerContext;
            _lobbyController = lobbyController;
        }

        public UIElement Index()
        {
            var view = new JoinControl();

            view.BackButtonClicked += BackButtonClicked;
            view.JoinButtonClicked += JoinButtonClicked;

            return view;
        }

        private void JoinButtonClicked(string playerName, string ipAddress, string port)
        {
            _playerContext.SetJoinParameters(ipAddress, port, playerName);

            _navigator.NavigateTo(() => _lobbyController.Index(false));
        }

        private void BackButtonClicked()
        {
            _navigator.NavigateFromHistory();
        }
    }
}