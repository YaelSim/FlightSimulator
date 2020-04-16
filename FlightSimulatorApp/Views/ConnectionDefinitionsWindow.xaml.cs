using FlightSimulatorApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FlightSimulatorApp.Views
{
    /// <summary>
    /// Interaction logic for ConnectionDefinitionsWindow.xaml
    /// </summary>
    public partial class ConnectionDefinitionsWindow : Window
    {
        private readonly ConnectionButtonsViewModel vm;
        public ConnectionDefinitionsWindow()
        {
            InitializeComponent();
            MainViewModel Main_VM = (Application.Current as App).MainVM;
            vm = Main_VM.ConnectionButtonsVM;
            DataContext = vm;
        }
        public void ClickedOnConnect(object sender, RoutedEventArgs e)
        {
            (Application.Current as App).MainWindowView = new MainWindow();
            (Application.Current as App).MainWindowView.Show();
            this.Close();
        }
    }
}
