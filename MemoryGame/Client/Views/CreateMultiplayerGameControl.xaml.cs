using System;
using System.Windows.Controls;
using System.Windows.Input;
using MemoryGame.Extensions;

namespace MemoryGame.Client.Views
{
    /// <summary>
    /// Interaction logic for CreateMultiplayerGameControl.xaml
    /// </summary>
    public partial class CreateMultiplayerGameControl : UserControl
    {
        public event Action<string> TextEnteredInChatbox;

        public CreateMultiplayerGameControl()
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
