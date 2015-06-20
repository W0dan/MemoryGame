using System;
using System.Windows;
using System.Windows.Controls;
using MemoryGame.Extensions;

namespace MemoryGame.Client.Views
{
    /// <summary>
    /// Interaction logic for NavigationControl.xaml
    /// </summary>
    public partial class NavigationControl : UserControl
    {
        public event Action BackButtonClicked;

        public NavigationControl()
        {
            InitializeComponent();
        }

        public void SetTitle(string title)
        {
            TitleLabel.Content = title;
        }

        private void BackButtonClick(object sender, RoutedEventArgs e)
        {
            BackButtonClicked.Raise();
        }
    }
}
