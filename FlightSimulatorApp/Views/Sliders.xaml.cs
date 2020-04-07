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
        //private readonly SlidersViewModel sliders_vm;
        private readonly JoystickViewModel joystick_vm;
        //public Sliders(SlidersViewModel vm)
        public Sliders(JoystickViewModel vm)
        {
            InitializeComponent();
            //this.sliders_vm = vm;
            //this.DataContext = this.sliders_vm;
            joystick_vm = vm;
            DataContext = joystick_vm;
        }
        public void ValueChangedAileron(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double val = Convert.ToDouble(e.NewValue);
            val *= 100;
            int valAsInt = (int)val;
            val = (double)valAsInt;
            val /= 100;
            //Convert the value (double) to string, in order to pass it to the ViewModel.
            string valAsString = val.ToString();
            string cmd = "set /controls/engines/current-engine/throttle " + valAsString;
            //this.sliders_vm.sendCommandToModel(cmd);
            joystick_vm.sendCommandToModel(cmd);
            //CHECK IF THE FOLLOWING LINE IS NEEDED *******************************
            //this.sliders_vm.VM_Aileron = val;
            joystick_vm.VM_Aileron = val;
        }
        public void ValueChangedThrottle(object sender, RoutedPropertyChangedEventArgs<double> e) {
            double val = Convert.ToDouble(e.NewValue);
            val *= 100;
            int valAsInt = (int)val;
            val = (double)valAsInt;
            val /= 100;
            //Convert the value (double) to string, in order to pass it to the ViewModel.
            string valAsString = val.ToString();
            string cmd = "set /controls/engines/current-engine/throttle " + valAsString;
           // this.sliders_vm.sendCommandToModel(cmd);
            //CHECK IF THE FOLLOWING LINE IS NEEDED *******************************
           // this.sliders_vm.VM_Throttle = val;
            joystick_vm.sendCommandToModel(cmd);
            joystick_vm.VM_Throttle = val;
        }
    }
}
