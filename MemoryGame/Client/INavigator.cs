using System;
using System.Windows;
using System.Windows.Controls;
using MemoryGame.Client.Views;

namespace MemoryGame.Client
{
    public interface INavigator
    {
        void Initialize(Grid layoutGrid, NavigationControl navigationControl);
        void NavigateTo(Func<UIElement> action);
        void NavigationCompleted(string title);
        void NavigateFromHistory();
    }
}