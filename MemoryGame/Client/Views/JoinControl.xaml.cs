using System;
using System.Windows;
using System.Windows.Controls;
using MemoryGame.Client.Extensions;

namespace MemoryGame.Client.Views
{
    /// <summary>
    /// Interaction logic for JoinControl.xaml
    /// </summary>
    public partial class JoinControl : UserControl
    {
        public event Action BackButtonClicked;
        public event Action<string, string, string> JoinButtonClicked;

        public JoinControl()
        {
            InitializeComponent();
        }

        private void JoinButtonClick(object sender, RoutedEventArgs e)
        {
            var ipAddress = IPAddressTextbox.Text;
            var port = PortTextbox.Text;
            var playerName = PlayerName.Text;

            JoinButtonClicked.Raise(playerName, ipAddress, port);
        }

        private void BackButtonClick(object sender, RoutedEventArgs e)
        {
            BackButtonClicked.Raise();
        }
    }
}
