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
    /// Interaction logic for Sliders.xaml
    /// </summary>
    public partial class Sliders : UserControl
    {
        private readonly FlightControlsViewModel vm;
        public Sliders()
        {
            InitializeComponent();
            if (Application.Current is App)
            {
                Main_VM = (Application.Current as App).MainVM;
                vm = Main_VM.FlightControlsVM;
            }
        }
        public void ValueChangedAileron(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double val = Convert.ToDouble(e.NewValue);
            val *= 1000;
            int valAsInt = (int)val;
            val = (double)valAsInt;
            val /= 1000;
            //Convert the value (double) to string, in order to pass it to the ViewModel.
            string valAsString = val.ToString();
            string cmd = "set /controls/flight/aileron " + valAsString;
            vm.SendCommandToModel(cmd);
            vm.VM_Aileron = val;
        }
        public void ValueChangedThrottle(object sender, RoutedPropertyChangedEventArgs<double> e) {
            double val = Convert.ToDouble(e.NewValue);
            val *= 1000;
            int valAsInt = (int)val;
            val = (double)valAsInt;
            val /= 1000;
            //Convert the value (double) to string, in order to pass it to the ViewModel.
            string valAsString = val.ToString();
            string cmd = "set /controls/engines/current-engine/throttle " + valAsString;
            vm.SendCommandToModel(cmd);
            vm.VM_Throttle = val;
        }
        public MainViewModel Main_VM { get; internal set; }

    }
}
