using FlightSimulatorApp.Model;
using FlightSimulatorApp.Utilities;
using FlightSimulatorApp.ViewModels;
using FlightSimulatorApp.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace FlightSimulatorApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public MainViewModel MainVM { get; internal set; }
        public MainWindow MainWindowView { get; set; }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            ISimulatorModel model = new MySimulatorModel(new TelnetClient());
            MainVM = new MainViewModel(model);

            ConnectionDefinitionsWindow window = new ConnectionDefinitionsWindow();
            window.Show();
        }
    }
}
