using System.Windows;
using MemoryGame.Client.Navigation;
using MemoryGame.Client.Views;

namespace MemoryGame.Client.Controllers
{
    public class MasterController : IMasterController
    {
        private readonly INavigator _navigator;
        private readonly IMenuController _menuController;

        public MasterController(INavigator navigator, IMenuController menuController)
        {
            _navigator = navigator;
            _menuController = menuController;
        }

        public UIElement Index()
        {
            var masterLayoutControl = new MasterLayoutControl();

            _navigator.Initialize(masterLayoutControl.LayoutGrid);

            _navigator.NavigateTo(() => _menuController.Index());

            return masterLayoutControl;
        }
    }
}