using System.Windows;
using MemoryGame.Client.Exceptions;
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
            try
            {
                _playerContext.SetJoinParameters(ipAddress, port, playerName);

                _navigator.NavigateTo(() => _lobbyController.Index(false));
            }
            catch (InvalidHostException ex)
            {
                _navigator.ShowMessage("An error occured", ex.Message);
            }
            catch (InvalidPortException ex)
            {
                _navigator.ShowMessage("An error occured", ex.Message);
            }
            catch
            {
                _navigator.ShowMessage("An error occured", "An unexpected exception occured");
            }
        }

        private void BackButtonClicked()
        {
            _navigator.NavigateFromHistory();
        }
    }
}