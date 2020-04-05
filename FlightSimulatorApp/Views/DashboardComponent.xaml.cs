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
    /// Interaction logic for DashboardComponent.xaml
    /// </summary>
    public partial class DashboardComponent : UserControl
    {
        private DashboardViewModel dashboard_vm;
        public DashboardComponent()
        {
            InitializeComponent();
            this.dashboard_vm = new DashboardViewModel();
            this.DataContext = this.dashboard_vm;
        }
    }
}
