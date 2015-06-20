﻿using System;
using System.Windows;
using System.Windows.Controls;
using MemoryGame.Client.Extensions;
using MemoryGame.Client.Models;

namespace MemoryGame.Client.Views
{
    /// <summary>
    /// Interaction logic for HostmenuControl.xaml
    /// </summary>
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
