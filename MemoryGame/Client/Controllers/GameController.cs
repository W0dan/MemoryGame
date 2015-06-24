using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using MemoryGame.Client.Service;
using MemoryGame.Client.Views;

namespace MemoryGame.Client.Controllers
{
    public class GameController : IGameController
    {
        private const string CardNameFormat = "card_{0}_{1}";
        private readonly IPlayerContext _playerContext;
        private GameControl _view;
        private Grid _cardsGrid;

        public GameController(IPlayerContext playerContext)
        {
            _playerContext = playerContext;
        }

        public UIElement Index(int rows, int columns)
        {
            _view = new GameControl();

            _cardsGrid = CreateCardsGrid(rows, columns);

            DrawCardsBacksides(rows, columns, _cardsGrid);

            _cardsGrid.IsEnabled = false;

            _playerContext.YourTurn += MyTurn;

            _playerContext.ReadyToRumble();

            return _view;
        }

        private void MyTurn()
        {
            _cardsGrid.Dispatcher.Invoke(() =>
            {
                _cardsGrid.IsEnabled = true;
            });
        }

        private Grid CreateCardsGrid(int rows, int columns)
        {
            var cardsGrid = new Grid();
            _view.LayoutGrid.Children.Add(cardsGrid);
            Grid.SetRow(cardsGrid, 1);

            for (var column = 0; column < columns; column++)
            {
                cardsGrid.ColumnDefinitions.Add(new ColumnDefinition
                {
                    Name = "cd_" + column
                });
            }
            for (var row = 0; row < rows; row++)
            {
                cardsGrid.RowDefinitions.Add(new RowDefinition
                {
                    Name = "rd_" + row
                });
            }
            return cardsGrid;
        }

        private void DrawCardsBacksides(int rows, int columns, Grid cardsGrid)
        {
            for (var row = 0; row < rows; row++)
            {
                for (var column = 0; column < columns; column++)
                {
                    var imageResource = Resources.ResourceHelper.GetImage("Client/Resources/Images/000.png");

                    var image = new Image
                    {
                        Name = string.Format(CardNameFormat, row, column),
                        Source = imageResource,
                        Stretch = Stretch.Uniform,
                        Margin = new Thickness(3),
                    };

                    image.MouseUp += CardClicked;

                    cardsGrid.Children.Add(image);
                    Grid.SetRow(image, row);
                    Grid.SetColumn(image, column);
                }
            }
        }

        private void CardClicked(object sender, MouseButtonEventArgs e)
        {
            var card = (Image)sender;

            var cardNameParts = card.Name.Split('_');

            var row = int.Parse(cardNameParts[1]);
            var column = int.Parse(cardNameParts[2]);

            _playerContext.CardClicked(row, column);
        }
    }
}