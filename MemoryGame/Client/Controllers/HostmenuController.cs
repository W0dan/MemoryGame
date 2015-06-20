using System.Windows;
using MemoryGame.Client.Navigation;
using MemoryGame.Client.Views;
using MemoryGame.Hosting;

namespace MemoryGame.Client.Controllers
{
    public class HostmenuController : IHostmenuController
    {
        private readonly INavigator _navigator;
        private readonly IHost _host;

        public HostmenuController(INavigator navigator, IHost host)
        {
            _navigator = navigator;
            _host = host;
        }

        public UIElement Index()
        {
            var view = new HostmenuControl();

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

            //todo: navigate to 'Start game' -screen
        }

        private void BackButtonClicked()
        {
            _navigator.NavigateFromHistory();
        }
    }
}