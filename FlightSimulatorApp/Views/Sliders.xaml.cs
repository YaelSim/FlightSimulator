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
        private readonly SlidersViewModel sliders_vm;
        public Sliders()
        {
            InitializeComponent();
            this.sliders_vm = new SlidersViewModel();
            this.DataContext = this.sliders_vm;
        }
        public void ValueChangedAileron(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double val = Convert.ToDouble(e.NewValue);
            this.sliders_vm.VM_Aileron = val;
        }
        public void ValueChangedThrottle(object sender, RoutedPropertyChangedEventArgs<double> e) {
            double val = Convert.ToDouble(e.NewValue);
            this.sliders_vm.VM_Throttle = val;
        }
    }
}
