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
            var masterController = new MasterController(new Navigator(), new MenuController());

            var masterLayout = masterController.Index();

            LayoutGrid.CreateContentControl(masterLayout);
        }
    }
}
