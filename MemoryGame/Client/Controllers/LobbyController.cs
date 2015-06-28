using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using MemoryGame.Client.Navigation;
using MemoryGame.Client.Service;
using MemoryGame.Client.Views;
using MemoryGame.Hosting;

namespace MemoryGame.Client.Controllers
{
    public class LobbyController : ILobbyController
    {
        private readonly INavigator _navigator;
        private readonly IHost _host;
        private readonly IPlayerContext _playerContext;
        private readonly IGameController _gameController;
        private LobbyControl _view;
        private string _selectedCardSet = "cars";
        readonly Dictionary<string, Border> _cardSetImages = new Dictionary<string, Border>();

        public LobbyController(INavigator navigator, IHost host, IPlayerContext playerContext, IGameController gameController)
        {
            _navigator = navigator;
            _host = host;
            _playerContext = playerContext;
            _gameController = gameController;
        }

        private void RefreshPlayerList(string player)
        {
            _view.PlayersStackpanel.Dispatcher.Invoke(() =>
            {
                var playerList = _playerContext.GetPlayerList();

                _view.PlayersStackpanel.Children.Clear();
                foreach (var p in playerList)
                {
                    _view.PlayersStackpanel.Children.Add(new Label { Content = p });
                }
            });
        }

        private void AppendToChatbox(string player, string message)
        {
            _view.ChatBox.Dispatcher.Invoke(() =>
            {
                _view.ChatBox.Content += string.Format("{0}> {1}\r\n", player, message);
            });
        }

        public UIElement Index(bool isHost)
        {
            Debug.WriteLine(_playerContext.PlayerName + " is on thread " + Thread.CurrentThread.ManagedThreadId);

            if (_view != null)
            {
                return _view;
            }

            _view = new LobbyControl();

            _playerContext.ChatMessageReceived += AppendToChatbox;
            _playerContext.PlayerJoined += RefreshPlayerList;
            _playerContext.GameStarted += GameStarted;

            _playerContext.Join();

            if (isHost)
            {
                _view.StartButton.Visibility = Visibility.Visible;
                _view.CancelButton.Content = "Cancel";
                _view.NumberOfCardsSlider.Visibility = Visibility.Visible;
                _view.CancelButtonClicked += CancelHosting;
                _view.StartButtonClicked += StartGame;
            }
            else
            {
                _view.CancelButtonClicked += LeaveGame;
            }

            _view.TextEnteredInChatbox += TextEnteredInChatbox;

            _view.CardsStackpanel.Children.Add(GetImage("cars", "001.png"));
            _view.CardsStackpanel.Children.Add(GetImage("princess", "003.png"));

            SelectCardSet("cars");

            return _view;
        }

        private void SelectCardSet(string cardSet)
        {
            foreach (var cardSetImage in _cardSetImages)
            {
                if (cardSetImage.Key == cardSet)
                {
                    cardSetImage.Value.BorderThickness = new Thickness(2);
                    cardSetImage.Value.BorderBrush = Brushes.Black;
                }
                else
                {
                    cardSetImage.Value.BorderThickness = new Thickness(0);
                    cardSetImage.Value.BorderBrush = Brushes.White;
                }
            }
        }

        private Border GetImage(string cardset, string imageName)
        {
            var imageSource = Resources.ResourceHelper.GetImageRelative(cardset, imageName);
            var image = new Image
            {
                Name = cardset,
                Source = imageSource,
                Width = 100,
                Height = 100
            };

            image.MouseUp += CardSetClicked;

            var border = new Border
            {
                Child = image
            };

            _cardSetImages.Add(cardset, border);

            return border;
        }

        private void CardSetClicked(object sender, MouseButtonEventArgs e)
        {
            var image = (Image)sender;

            _selectedCardSet = image.Name;
            SelectCardSet(_selectedCardSet);
        }

        private void GameStarted(int rows, int columns)
        {
            _navigator.NavigateTo(() => _gameController.Index(rows, columns));
        }

        private void StartGame(int numberOfCardsLevel)
        {
            //todo: send a message to server to stop accepting new players
            //todo: and also to stop accepting chat messages

            _playerContext.StartGame(_selectedCardSet, numberOfCardsLevel);
        }

        private void LeaveGame()
        {
            //todo: send Leave() message to server
            _navigator.NavigateFromHistory();
        }

        private void CancelHosting()
        {
            //todo: notify all players that the host is about to stop
            _host.Stop();

            _navigator.NavigateFromHistory();
        }

        private void TextEnteredInChatbox(string text)
        {
            _playerContext.SendChatMessage(text);

            _view.ChatTextbox.Text = "";
        }
    }
}