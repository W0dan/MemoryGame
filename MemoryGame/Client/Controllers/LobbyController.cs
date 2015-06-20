using System.Windows;
using System.Windows.Controls;
using MemoryGame.Client.Service;
using MemoryGame.Client.Views;

namespace MemoryGame.Client.Controllers
{
    public class LobbyController : ILobbyController
    {
        private readonly IPlayerContext _playerContext;
        private LobbyControl _view;

        public LobbyController(IPlayerContext playerContext)
        {
            _playerContext = playerContext;

            _playerContext.ChatMessageReceived += AppendToChatbox;
            _playerContext.PlayerJoined += RefreshPlayerList;
        }

        private void RefreshPlayerList(string player)
        {
            var playerList = _playerContext.GetPlayerList();

            _view.PlayersStackpanel.Children.Clear();
            foreach (var p in playerList)
            {
                _view.PlayersStackpanel.Children.Add(new Label {Content = p});
            }
        }

        private void AppendToChatbox(string player, string message)
        {
            _view.ChatBox.Content += string.Format("{0}> {1}\r\n", player, message);
        }

        public UIElement Index()
        {
            _view = new LobbyControl();

            _view.TextEnteredInChatbox += TextEnteredInChatbox;

            RefreshPlayerList(_playerContext.PlayerName);

            return _view;
        }

        private void TextEnteredInChatbox(string text)
        {
            _playerContext.SendChatMessage(text);

            AppendToChatbox(_playerContext.PlayerName, text);
            _view.ChatTextbox.Text = "";
        }
    }
}