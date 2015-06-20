using System;
using System.Windows;
using System.Windows.Controls;
using MemoryGame.Client.Models;
using MemoryGame.Extensions;

namespace MemoryGame.Client.Views
{
    public partial class HostmenuControl : UserControl
    {
        public event Action BackButtonClicked;
        public event Action<string, string> StartHostingButtonClicked;

        public HostmenuControl(HostmenuModel model)
        {
            InitializeComponent();

            IPAddressLabel.Content = model.LocalIPAddress;
        }

        private void BackButtonClick(object sender, RoutedEventArgs e)
        {
            BackButtonClicked.Raise();
        }

        private void StartHostingButtonClick(object sender, RoutedEventArgs e)
        {
            var port = PortTextbox.Text;
            var playerName = PlayerName.Text;

            StartHostingButtonClicked.Raise(playerName, port);
        }
    }
}
