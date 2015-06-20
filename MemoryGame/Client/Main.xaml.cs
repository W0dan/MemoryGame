using System.Windows;
using MemoryGame.Client.Controllers;
using MemoryGame.Client.Extensions;
using MemoryGame.Client.Navigation;
using MemoryGame.Client.Service;
using MemoryGame.Hosting;

namespace MemoryGame.Client
{
    public partial class Main : Window
    {
        public Main()
        {
            InitializeComponent();

            CreateMasterLayout();
        }

        private void CreateMasterLayout()
        {
            //some bootstrapping going on here:
            //(poor man's di)
            var navigator = new Navigator();

            var host = new Host();
            var ipAddressProvider = new IPAddressProvider();
            var playerContext = new PlayerContext();
            var lobbyController = new LobbyController(navigator, host, playerContext);
            var hostmenuController = new HostmenuController(navigator, host, ipAddressProvider, lobbyController, playerContext);
            var joinController = new JoinController(navigator, playerContext, lobbyController);
            var multiplayermenuController = new MultiplayermenuController(navigator, hostmenuController, joinController);
            var mainmenuController = new MainmenuController(navigator, multiplayermenuController);
            var masterController = new MasterController(navigator, mainmenuController);

            var masterLayout = masterController.Index();

            LayoutGrid.CreateContentControl(masterLayout);
        }
    }
}
