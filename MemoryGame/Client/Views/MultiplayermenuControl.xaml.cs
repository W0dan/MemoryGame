using System;
using System.Windows;
using System.Windows.Controls;
using MemoryGame.Extensions;

namespace MemoryGame.Client.Views
{
    /// <summary>
    /// Interaction logic for MultiplayermenuControl.xaml
    /// </summary>
    public partial class MultiplayermenuControl : UserControl
    {
        public event Action HostButtonClicked;
        public event Action JoinButtonClicked;
        public event Action BackButtonClicked;

        public MultiplayermenuControl()
        {
            InitializeComponent();
        }

        private void HostButtonClick(object sender, RoutedEventArgs e)
        {
            HostButtonClicked.Raise();
        }

        private void JoinButtonClick(object sender, RoutedEventArgs e)
        {
            JoinButtonClicked.Raise();
        }

        private void BackButtonClick(object sender, RoutedEventArgs e)
        {
            BackButtonClicked.Raise();
        }
    }
}
