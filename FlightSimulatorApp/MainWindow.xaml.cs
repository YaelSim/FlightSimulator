using FlightSimulatorApp.Model;
using FlightSimulatorApp.Utilities;
using FlightSimulatorApp.Views;
using FlightSimulatorApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace FlightSimulatorApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(string ip, int port)
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            Main_VM = (Application.Current as App).MainVM;
            DataContext = Main_VM;
            InitializeComponent();
            //Constructs all of the views' fields.
            SetViews();
            Main_VM.Connect(ip, port);
            Main_VM.Start();
        }
        private void SetViews()
        {
            ConnectionButtons.DataContext = Main_VM.ConnectionButtonsVM;
            Dashboard.DataContext = Main_VM.DashboardVM;
            MapComponent.DataContext = Main_VM.MapVM;
            FlightControls.DataContext = Main_VM.FlightControlsVM;
        }
        public MainViewModel Main_VM { get; internal set; }
    }
}
