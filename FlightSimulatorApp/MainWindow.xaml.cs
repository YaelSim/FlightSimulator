using FlightSimulatorApp.Model;
using FlightSimulatorApp.Utilities;
using FlightSimulatorApp.Views;
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
using System.Windows.Shapes;

namespace FlightSimulatorApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            /*ITelnet tc = new TelnetClient();
            ISimulatorModel model = new MySimulatorModel(tc);
            model.connect("127.0.0.1", 5402);
            model.start();
            model.disconnect();*/
            MapComponent map = new MapComponent();
        }
    }
}
