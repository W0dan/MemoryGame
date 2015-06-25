using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using MemoryGame.Client.Service;
using MemoryGame.Client.Views;
using MemoryGame.Contracts;

namespace MemoryGame.Client.Controllers
{
    public class GameController : IGameController
    {
        private const string CardNameFormat = "card_{0}_{1}";
        private readonly IPlayerContext _playerContext;
        private GameControl _view;
        private Grid _cardsGrid;
        private Dictionary<string, Image> _cardsImages = new Dictionary<string, Image>();

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
            _playerContext.PlayerIsOnTurn += PlayerIsOnTurn;
            _playerContext.FirstCardSelected += FirstCardSelected;
            _playerContext.SecondCardSelected += SecondCardSelected;
            _playerContext.SecondCardMatches += SecondCardMatches;
            _playerContext.SecondCardDoesntMatch += SecondCardDoesntMatch;

            _playerContext.ReadyToRumble();

            return _view;
        }

        private void FirstCardSelected(SelectedCard card)
        {
            _cardsGrid.Dispatcher.Invoke(() =>
            {
                //get image containing this card
                var cardName = string.Format(CardNameFormat, card.Row, card.Column);
                var image = _cardsImages[cardName];

                //change the image to the resourceindex
                var imageResource = Resources.ResourceHelper.GetImage(string.Format("Client/Resources/Images/{0:000}.png", card.ResourceIndex));
                image.Source = imageResource;
            });
        }

        private void SecondCardSelected(SelectedCard card)
        {
            _cardsGrid.Dispatcher.Invoke(() =>
            {
                FlipCard(card, card.ResourceIndex);
            });
        }

        private void FlipCard(SelectedCard card, int resourceIndex)
        {
            //get image containing this card
            var cardName = string.Format(CardNameFormat, card.Row, card.Column);
            var image = _cardsImages[cardName];

            //change the image to the resourceindex
            var imageResource = Resources.ResourceHelper.GetImage(string.Format("Client/Resources/Images/{0:000}.png", resourceIndex));
            image.Source = imageResource;
        }

        private void SecondCardMatches(SelectedCard firstCard, SelectedCard secondCard)
        {
            _cardsGrid.Dispatcher.Invoke(() =>
            {
                RemoveCard(firstCard);
                RemoveCard(secondCard);
            });
        }

        private void RemoveCard(SelectedCard card)
        {
            var cardName = string.Format(CardNameFormat, card.Row, card.Column);
            var image = _cardsImages[cardName];

            _cardsGrid.Children.Remove(image);
        }

        private void SecondCardDoesntMatch(SelectedCard firstCard, SelectedCard secondCard)
        {
            _cardsGrid.Dispatcher.Invoke(() =>
            {
                FlipCard(firstCard, 0);
                FlipCard(secondCard, 0);
            });
        }

        private void PlayerIsOnTurn(string player)
        {

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
                cardsGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }
            for (var row = 0; row < rows; row++)
            {
                cardsGrid.RowDefinitions.Add(new RowDefinition());
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

                    var cardName = string.Format(CardNameFormat, row, column);
                    var image = new Image
                    {
                        Name = cardName,
                        Source = imageResource,
                        Stretch = Stretch.Uniform,
                        Margin = new Thickness(3),
                    };

                    image.MouseUp += CardClicked;

                    _cardsImages.Add(cardName, image);

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