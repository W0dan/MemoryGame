using System.Windows;
using MemoryGame.Client.Controllers;
using MemoryGame.Client.Extensions;
using MemoryGame.Client.Navigation;

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
            var navigator = new Navigator();
            var mainmenuController = new MainmenuController(navigator, new MultiplayermenuController(navigator));
            var masterController = new MasterController(navigator, mainmenuController);

            var masterLayout = masterController.Index();

            LayoutGrid.CreateContentControl(masterLayout);
        }
    }
}
