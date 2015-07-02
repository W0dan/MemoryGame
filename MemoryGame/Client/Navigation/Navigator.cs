using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using MemoryGame.Client.Extensions;
using MemoryGame.Client.Views;

namespace MemoryGame.Client.Navigation
{
    public class Navigator : INavigator
    {
        private static UIElement _activeControl;
        private static Grid _layoutGrid;

        private static readonly Stack<Func<UIElement>> NavigationHistory = new Stack<Func<UIElement>>();
        private readonly MessageBoxControl _messageBox;

        public Navigator()
        {
            _messageBox = new MessageBoxControl();

            _messageBox.OkButton.Click += MessageClicked;
        }

        private void MessageClicked(object sender, RoutedEventArgs routedEventArgs)
        {
            _layoutGrid.Dispatcher.Invoke(() =>
            {
                _layoutGrid.Children.Remove(_messageBox);
            });
        }

        public void Initialize(Grid layoutGrid)
        {
            _layoutGrid = layoutGrid;
        }

        public void NavigateTo(Func<UIElement> action, string navigationErrorMessage = "An unknown error occured")
        {
            NavigationHistory.Push(action);

            TryActivateAction(action, navigationErrorMessage);
        }

        public void NavigateFromHistory()
        {
            if (NavigationHistory.Count == 1) return;

            NavigationHistory.Pop();
            var action = NavigationHistory.Peek();

            ActivateAction(action);
        }

        public void ShowMessage(string title, string message)
        {
            _layoutGrid.Dispatcher.Invoke(() =>
            {
                _messageBox.TitleLabel.Content = title;
                _messageBox.ContentLabel.Text = message;
                _messageBox.Width = 400;
                _messageBox.Height = 250;

                _layoutGrid.CreateContentControl(_messageBox);
            });
        }

        private static void ActivateAction(Func<UIElement> action)
        {
            _layoutGrid.Dispatcher.Invoke(() =>
            {
                if (_activeControl != null)
                    _layoutGrid.Children.Remove(_activeControl);

                _activeControl = action();
                _layoutGrid.CreateContentControl(_activeControl);
            });
        }

        private void TryActivateAction(Func<UIElement> action, string navigationErrorMessage)
        {
            _layoutGrid.Dispatcher.Invoke(() =>
            {
                if (_activeControl != null)
                    _layoutGrid.Children.Remove(_activeControl);

                try
                {
                    _activeControl = action();
                    _layoutGrid.CreateContentControl(_activeControl);
                }
                catch (Exception ex)
                {
                    NavigateFromHistory();
                    ShowMessage("An error occured", navigationErrorMessage ?? ex.ToString());
                }
            });
        }
    }
}