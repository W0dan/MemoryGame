using System.Windows;
using System.Windows.Controls;
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

        public LobbyController(INavigator navigator, IHost host, IPlayerContext playerContext, IGameController gameController)
        {
            _navigator = navigator;
            _host = host;
            _playerContext = playerContext;
            _gameController = gameController;
        }

        private void RefreshPlayerList(string player)
        {
            var playerList = _playerContext.GetPlayerList();

            _view.PlayersStackpanel.Children.Clear();
            foreach (var p in playerList)
            {
                _view.PlayersStackpanel.Children.Add(new Label { Content = p });
            }
        }

        private void AppendToChatbox(string player, string message)
        {
            _view.ChatBox.Content += string.Format("{0}> {1}\r\n", player, message);
        }

        public UIElement Index(bool isHost)
        {
            _view = new LobbyControl();

            _playerContext.ChatMessageReceived += AppendToChatbox;
            _playerContext.PlayerJoined += RefreshPlayerList;

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
                _playerContext.GameStarted += GameStarted;
            }

            _view.TextEnteredInChatbox += TextEnteredInChatbox;

            RefreshPlayerList(_playerContext.PlayerName);

            return _view;
        }

        private void GameStarted(int rows, int columns)
        {
            _navigator.NavigateTo(() => _gameController.Index(rows, columns));
        }

        private void StartGame(int rows, int columns)
        {
            //todo: send a message to server to stop accepting new players
            //todo: and also to stop accepting chat messages
            //todo: and to start an instance of the game core

            _playerContext.StartGame(rows, columns);

            _navigator.NavigateTo(() => _gameController.Index(rows, columns));
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

            AppendToChatbox(_playerContext.PlayerName, text);
            _view.ChatTextbox.Text = "";
        }
    }
}