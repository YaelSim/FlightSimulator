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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FlightSimulatorApp.Views
{
    /// <summary>
    /// Interaction logic for ConnectionButtons.xaml
    /// </summary>
    public partial class ConnectionButtons : UserControl
    {
        private ConnectionButtonsViewModel connection_vm;
        public ConnectionButtons()
        {
            InitializeComponent();
            this.connection_vm = new ConnectionButtonsViewModel();
            this.DataContext = this.connection_vm;
        }
        void OnClickConnect(object sender, RoutedEventArgs e)
        {
            ConnectButton.Foreground = new SolidColorBrush(Colors.Green);
        }
        void OnClickDisconnect(object sender, RoutedEventArgs e)
        {
            DisconnectButton.Foreground = new SolidColorBrush(Colors.Red);
        }
    }
}
