using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MemoryGame.Extensions;

namespace MemoryGame.Client.Views
{
    public partial class LobbyControl : UserControl
    {
        public event Action<string> TextEnteredInChatbox;
        public event Action CancelButtonClicked;
        public event Action<int, int> StartButtonClicked;

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

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            CancelButtonClicked.Raise();
        }

        private void StartButtonClick(object sender, RoutedEventArgs e)
        {
            var numberOfCardsLevel = NumberOfCardsSlider.Value;

            var rows = (int)numberOfCardsLevel + 1;
            var columns = (int)numberOfCardsLevel + 2;

            StartButtonClicked.Raise(rows, columns);
        }
    }
}
