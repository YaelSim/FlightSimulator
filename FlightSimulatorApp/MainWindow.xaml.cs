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
        private DashboardComponent dashboard_view;
        private MapComponent map_view;
        private Joystick joystick_view;
        private Sliders sliders_view;
        private ConnectionButtons connectionButtons_view;
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
            /*MapComponent map_view = new MapComponent();
            map_view.DataContext = Main_VM.MapVM;
            dashboard_view = new DashboardComponent();
            dashboard_view.DataContext = Main_VM.DashboardVM;
            joystick_view = new Joystick();
            joystick_view.DataContext = Main_VM.JoystickVM;
            sliders_view = new Sliders();
            sliders_view.DataContext = Main_VM.JoystickVM;
            connectionButtons_view = new ConnectionButtons();
            connectionButtons_view.DataContext = Main_VM.ConnectionButtonsVM;
            */
            ConnectionButtons.DataContext = Main_VM.ConnectionButtonsVM;
            Dashboard.DataContext = Main_VM.DashboardVM;
            FlightControls.DataContext = Main_VM.JoystickVM;
        }
        public MainViewModel Main_VM { get; internal set; }
    }
}
