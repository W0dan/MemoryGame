using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using MemoryGame.Client.Extensions;

namespace MemoryGame.Client.Navigation
{
    public class Navigator : INavigator
    {
        private static UIElement _activeControl;
        private static Grid _layoutGrid;

        private static readonly Stack<Func<UIElement>> NavigationHistory = new Stack<Func<UIElement>>();

        public void Initialize(Grid layoutGrid)
        {
            _layoutGrid = layoutGrid;
        }

        public void NavigateTo(Func<UIElement> action)
        {
            NavigationHistory.Push(action);

            ActivateAction(action);
        }

        public void NavigateFromHistory()
        {
            if (NavigationHistory.Count == 1) return;

            NavigationHistory.Pop();
            var action = NavigationHistory.Peek();

            ActivateAction(action);
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
    }
}