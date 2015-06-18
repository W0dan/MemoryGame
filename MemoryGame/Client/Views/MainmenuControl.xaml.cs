using System;
using System.Windows;
using System.Windows.Controls;
using MemoryGame.Client.Extensions;

namespace MemoryGame.Client.Views
{
    public partial class MainmenuControl : UserControl
    {
        public event Action ExitButtonClicked;

        public MainmenuControl()
        {
            InitializeComponent();
        }

        private void SingleplayerButtonClick(object sender, RoutedEventArgs e)
        {
            
        }

        private void MultiplayerButtonClick(object sender, RoutedEventArgs e)
        {
            
        }

        private void ExitButtonClick(object sender, RoutedEventArgs e)
        {
            ExitButtonClicked.Raise();
        }
    }
}
