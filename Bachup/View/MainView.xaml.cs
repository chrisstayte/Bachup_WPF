using System;
using System.Windows;
using System.Windows.Input;

namespace Bachup.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();
        }

        private void ColorZone_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
                
        }

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Maximize_Click(object sender, RoutedEventArgs e)
        {
            MaximizeToggle();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            //Environment.Exit(0);
            base.Close();
        }

        private void TreeViewItem_Expanded_Collapsed(object sender, RoutedEventArgs e)
        {
            Bachup.ViewModel.MainViewModel.SaveSettings();
            Bachup.ViewModel.MainViewModel.SaveData();
        }

        private void ColorZone_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MaximizeToggle();
        }

        private void MaximizeToggle()
        {
            if (WindowState.ToString() == "Normal")
            {
                this.WindowState = WindowState.Maximized;
            }
            else
            {
                this.WindowState = WindowState.Normal;
            }
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (WindowState.ToString() == "Normal")
            {
                this.BorderThickness = new Thickness(0);
            }
            else
            {
                this.BorderThickness = new Thickness(5);
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        public void ExitApplication()
        {
            base.Close();
        }

    }
}
