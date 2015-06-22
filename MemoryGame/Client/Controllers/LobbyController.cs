﻿using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using MemoryGame.Client.Navigation;
using MemoryGame.Client.Service;
using MemoryGame.Client.Views;
using MemoryGame.Hosting;

namespace MemoryGame.Client.Controllers
{
    public class LobbyController : ILobbyController
    {
        private readonly INavigator _navigator;
        private readonly IHost _host;
        private readonly IPlayerContext _playerContext;
        private LobbyControl _view;

        public LobbyController(INavigator navigator, IHost host, IPlayerContext playerContext)
        {
            _navigator = navigator;
            _host = host;
            _playerContext = playerContext;
        }

        private void RefreshPlayerList(string player)
        {
            _view.PlayersStackpanel.Dispatcher.Invoke(() =>
            {
                var playerList = _playerContext.GetPlayerList();

                _view.PlayersStackpanel.Children.Clear();
                foreach (var p in playerList)
                {
                    _view.PlayersStackpanel.Children.Add(new Label { Content = p });
                }
            });
        }

        private void AppendToChatbox(string player, string message)
        {
            _view.ChatBox.Dispatcher.Invoke(() =>
            {
                _view.ChatBox.Content += string.Format("{0}> {1}\r\n", player, message);
            });
        }

        public UIElement Index(bool isHost)
        {
            Debug.WriteLine(_playerContext.PlayerName + " is on thread " + Thread.CurrentThread.ManagedThreadId);

            _view = new LobbyControl();

            _playerContext.ChatMessageReceived += AppendToChatbox;
            _playerContext.PlayerJoined += RefreshPlayerList;

            _playerContext.Join();

            if (isHost)
            {
                _view.StartButton.Visibility = Visibility.Visible;
                _view.CancelButton.Content = "Cancel";
                _view.NumberOfCardsSlider.Visibility = Visibility.Visible;
                _view.CancelButtonClicked += CancelHosting;
            }
            else
            {
                _view.CancelButtonClicked += LeaveGame;
            }

            _view.TextEnteredInChatbox += TextEnteredInChatbox;

            return _view;
        }

        private void LeaveGame()
        {
            //todo: send Leave() message to server
            _navigator.NavigateFromHistory();
        }

        private void CancelHosting()
        {
            //todo: notify all players that the host is about to stop
            _host.Stop();

            _navigator.NavigateFromHistory();
        }

        private void TextEnteredInChatbox(string text)
        {
            _playerContext.SendChatMessage(text);

            _view.ChatTextbox.Text = "";
        }
    }
}