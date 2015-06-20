using System.Windows;
using MemoryGame.Client.Extensions;
using MemoryGame.Client.Models;
using MemoryGame.Client.Navigation;
using MemoryGame.Client.Service;
using MemoryGame.Client.Views;
using MemoryGame.Hosting;

namespace MemoryGame.Client.Controllers
{
    public class HostmenuController : IHostmenuController
    {
        private readonly INavigator _navigator;
        private readonly IHost _host;
        private readonly IIPAddressProvider _ipAddressProvider;
        private readonly ICreateMultiplayerGameController _createMultiplayerGameController;
        private readonly IPlayerContext _playerContext;

        public HostmenuController(INavigator navigator, IHost host, IIPAddressProvider ipAddressProvider, ICreateMultiplayerGameController createMultiplayerGameController, IPlayerContext playerContext)
        {
            _navigator = navigator;
            _host = host;
            _ipAddressProvider = ipAddressProvider;
            _createMultiplayerGameController = createMultiplayerGameController;
            _playerContext = playerContext;
        }

        public UIElement Index()
        {
            var ipAddress = _ipAddressProvider.GetLocalIPAddress();

            var view = new HostmenuControl(new HostmenuModel
            {
                LocalIPAddress = ipAddress.ToString()
            });

            view.BackButtonClicked += BackButtonClicked;
            view.StartHostingButtonClicked += StartHostingButtonClicked;

            return view;
        }

        private void StartHostingButtonClicked(string playerName, string port)
        {
            int intPort;

            if (!int.TryParse(port, out intPort))
            {
                //todo: display 'invalid port' error
                return;
            }

            _host.Start(playerName, intPort);

            _playerContext.Join(_ipAddressProvider.GetLocalIPAddress().ToString(), port, playerName);

            _navigator.NavigateTo(_createMultiplayerGameController.Index);
        }

        private void BackButtonClicked()
        {
            _navigator.NavigateFromHistory();
        }
    }
}