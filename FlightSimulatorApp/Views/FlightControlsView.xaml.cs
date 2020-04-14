﻿using FlightSimulatorApp.ViewModels;
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
    /// Interaction logic for FlightControlsView.xaml
    /// </summary>
    public partial class FlightControlsView : UserControl
    {
        private FlightControlsViewModel vm;
        public FlightControlsView()
        {
            InitializeComponent();
            if (Application.Current is App)
            {
                Main_VM = (Application.Current as App).MainVM;
                vm = Main_VM.FlightControlsVM;
                Joystick.DataContext = vm;
                //Joystick.MouseMove += Joystick_MouseMove;
                Sliders.DataContext = vm;
            }
        }
        private void Joystick_MouseMove(object sender, MouseEventArgs e)
        {
            bool mouseIsDown = System.Windows.Input.Mouse.LeftButton == MouseButtonState.Pressed;
            if (mouseIsDown)
            {
                vm.VM_Rudder = Joystick.X;
                vm.VM_Elevator = Joystick.Y;
            }
        }

        public MainViewModel Main_VM { get; internal set; }
    }
}
