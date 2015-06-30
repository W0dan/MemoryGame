using System;
using System.Windows;
using System.Windows.Controls;

namespace MemoryGame.Client.Navigation
{
    public interface INavigator
    {
        void Initialize(Grid layoutGrid);
        void NavigateTo(Func<UIElement> action);
        void NavigateFromHistory();
        void ShowMessage(string title, string message);
    }
}