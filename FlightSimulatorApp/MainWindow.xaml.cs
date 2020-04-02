using FlightSimulatorApp.Utilities;
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
            ITelnet tc = new TelnetClient();
            tc.connect("127.0.0.1", 5402);
            tc.write("Hello world");
            Console.WriteLine(tc.read());
            tc.disconnect();
            InitializeComponent();
        }
        /*public static int Main(string[] args)
        {
            ITelnet tc = new TelnetClient();
            tc.connect("127.0.0.1", 5402);
            tc.write("Hello world");
            tc.read();
            tc.disconnect();
            return 0;
        }*/
    }
}
