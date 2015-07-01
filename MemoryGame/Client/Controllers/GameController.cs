using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using MemoryGame.Client.Navigation;
using MemoryGame.Client.Resources;
using MemoryGame.Client.Service;
using MemoryGame.Client.Views;
using MemoryGame.Contracts;

namespace MemoryGame.Client.Controllers
{
    public class GameController : IGameController
    {
        private const string CardNameFormat = "card_{0}_{1}";

        private readonly INavigator _navigator;
        private readonly IPlayerContext _playerContext;
        private GameControl _view;
        private Grid _cardsGrid;

        private readonly Dictionary<string, Image> _cardsImages = new Dictionary<string, Image>();
        private readonly Dictionary<string, Label> _playersLabels = new Dictionary<string, Label>();

        public GameController(INavigator navigator, IPlayerContext playerContext)
        {
            _navigator = navigator;
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
            _playerContext.PlayerReceivesPoints += PlayerReceivesPoints;
            _playerContext.Victory += Victory;
            _playerContext.Defeat += Defeat;

            _view.PlayerLabel.Content = _playerContext.PlayerName;

            LoadPlayers();

            _playerContext.ReadyToRumble();

            return _view;
        }

        private void Defeat()
        {
            ShowOutcome(ImageResources.Defeat);
        }

        private void Victory()
        {
            ShowOutcome(ImageResources.Victory);
        }

        private void ShowOutcome(ImageResources outcome)
        {
            _view.OutCome.Dispatcher.Invoke(() =>
            {
                var imageSource = ResourceHelper.GetImage(outcome);

                _view.OutCome.MouseUp += OutcomeClicked;

                _view.OutCome.Visibility = Visibility.Visible;
                _view.OutCome.Source = imageSource;
            });
        }

        private void OutcomeClicked(object sender, MouseButtonEventArgs e)
        {
            _navigator.NavigateFromHistory();
        }

        private void PlayerReceivesPoints(string player, int points)
        {
            var label = _playersLabels[player];
            label.Dispatcher.Invoke(() =>
            {
                label.Content = string.Format("{0} [ {1} ]", player, points);
            });
        }

        private void LoadPlayers()
        {
            _playersLabels.Clear();

            var players = _playerContext.GetPlayerList();
            foreach (var player in players)
            {
                var label = new Label
                {
                    Name = player,
                    Content = player,
                    FontSize = 15,
                    Margin = new Thickness(5, 2, 5, 2),
                    Width = 150
                };
                _playersLabels.Add(player, label);
                _view.PlayersStackpanel.Children.Add(label);
            }
        }

        private void FirstCardSelected(SelectedCard card)
        {
            _cardsGrid.Dispatcher.Invoke(() =>
            {
                FlipCard(card, card.ResourceIndex);
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
            var imageResource = ResourceHelper.GetImage(card.ResourceSet, resourceIndex);
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
            _view.Dispatcher.Invoke(() =>
            {
                foreach (var playersLabel in _playersLabels)
                {
                    if (playersLabel.Key == player)
                    {
                        playersLabel.Value.FontSize = 20;
                        playersLabel.Value.FontWeight = FontWeights.Bold;
                    }
                    else
                    {
                        playersLabel.Value.FontSize = 15;
                        playersLabel.Value.FontWeight = FontWeights.Normal;
                    }
                }
            });
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
            Grid.SetRow(cardsGrid, 2);

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

        private void DrawCardsBacksides(int rows, int columns, Panel cardsGrid)
        {
            _cardsImages.Clear();

            for (var row = 0; row < rows; row++)
            {
                for (var column = 0; column < columns; column++)
                {
                    var imageResource = ResourceHelper.GetImage(ImageResources.CardBackside);

                    var cardName = string.Format(CardNameFormat, row, column);
                    var image = new Image
                    {
                        Name = cardName,
                        Source = imageResource,
                        Stretch = Stretch.Uniform,
                        Margin = new Thickness(3),
                    };

                    image.MouseUp += CardClicked;

                    _cardsImages[cardName] = image;

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