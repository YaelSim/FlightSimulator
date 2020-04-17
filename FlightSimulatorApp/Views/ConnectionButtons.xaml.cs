using FlightSimulatorApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private readonly ConnectionButtonsViewModel vm;
        public ConnectionButtons()
        {
            InitializeComponent();
            if (Application.Current is App)
            {
                Main_VM = (Application.Current as App).MainVM;
                vm = Main_VM.ConnectionButtonsVM;
            }
        }
        void OnClickConnect(object sender, RoutedEventArgs e)
        {
            ConnectButton.Foreground = new SolidColorBrush(Colors.Green);
            ConnectionDefinitionsWindow window = new ConnectionDefinitionsWindow();
            window.Show();
            //Close the current window!
            if (Application.Current is App)
            {
                (Application.Current as App).MainWindowView.Close();
            }
        }
        void OnClickDisconnect(object sender, RoutedEventArgs e)
        {
            DisconnectButton.Foreground = new SolidColorBrush(Colors.Red);
            if (!vm.VM_CurrStatus.Equals("Disconnected"))
            {
                vm.VM_CurrStatus = "Disconnected";
                vm.Disconnect();
            }
        }
        void OnClickExit(object sender, RoutedEventArgs e)
        {
            if (Application.Current is App)
            {
                if (!vm.VM_CurrStatus.Equals("Disconnected"))
                {
                    vm.Disconnect();
                }
                Application.Current.Shutdown();
            }
        }
        public MainViewModel Main_VM { get; internal set; }

    }
}
