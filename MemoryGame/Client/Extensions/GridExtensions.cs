using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MemoryGame.Client.Extensions
{
    public static class GridExtensions
    {
        public static void CreateContentControl(this Grid layoutGrid, UIElement contentControl, int column = 0, int row = 0, int columnSpan = 1, int rowSpan = 1)
        {
            contentControl.Focusable = true;

            Grid.SetColumn(contentControl, column);
            Grid.SetRow(contentControl, row);
            Grid.SetColumnSpan(contentControl, columnSpan);
            Grid.SetRowSpan(contentControl, rowSpan);

            layoutGrid.Children.Add(contentControl);

            FocusManager.SetFocusedElement(layoutGrid, contentControl);
            Keyboard.Focus(contentControl);
        }
    }
}