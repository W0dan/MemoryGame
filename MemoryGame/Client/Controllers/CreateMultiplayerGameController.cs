using System.Windows;
using MemoryGame.Client.Service;
using MemoryGame.Client.Views;

namespace MemoryGame.Client.Controllers
{
    public class CreateMultiplayerGameController : ICreateMultiplayerGameController
    {
        private readonly IPlayerContext _playerContext;
        private CreateMultiplayerGameControl _view;

        public CreateMultiplayerGameController(IPlayerContext playerContext)
        {
            _playerContext = playerContext;

            _playerContext.ChatMessageReceived += AppendToChatbox;
        }

        private void AppendToChatbox(string player, string message)
        {
            _view.ChatBox.Content += string.Format("{0}> {1}\r\n", player, message);
        }

        public UIElement Index()
        {
            _view = new CreateMultiplayerGameControl();

            _view.TextEnteredInChatbox += TextEnteredInChatbox;

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