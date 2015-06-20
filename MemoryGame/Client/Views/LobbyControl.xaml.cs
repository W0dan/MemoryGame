using System;
using System.Windows.Controls;
using System.Windows.Input;
using MemoryGame.Extensions;

namespace MemoryGame.Client.Views
{
     public partial class LobbyControl : UserControl
    {
        public event Action<string> TextEnteredInChatbox;

        public LobbyControl()
        {
            InitializeComponent();
        }

        private void ChatTextboxOnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return || e.Key == Key.Enter)
            {
                TextEnteredInChatbox.Raise(ChatTextbox.Text);
            }
        }
    }
}
