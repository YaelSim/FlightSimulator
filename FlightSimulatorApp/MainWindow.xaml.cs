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
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            Main_VM = (Application.Current as App).MainVM;
            DataContext = Main_VM;
            InitializeComponent();

            Main_VM.Connect(ip, port);
            Main_VM.Start();
        }
        public MainViewModel Main_VM { get; internal set; }
    }
}
