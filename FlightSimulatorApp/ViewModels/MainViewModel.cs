using FlightSimulatorApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace FlightSimulatorApp.ViewModels
{
    public partial class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private ISimulatorModel model;
        private ConnectionButtonsViewModel ConnectionButtons_vm;
        private DashboardViewModel Dashboard_vm;
        //private SlidersViewModel Sliders_vm;
        private MapViewModel Map_vm;
        private JoystickViewModel Joystick_vm;
        public MainViewModel(ISimulatorModel m)
        {
            model = m;
            ConnectionButtons_vm = new ConnectionButtonsViewModel(this.model);
            Dashboard_vm = new DashboardViewModel(this.model);
            //Sliders_vm = new SlidersViewModel(this.model);
            Map_vm = new MapViewModel(this.model);
            Joystick_vm = new JoystickViewModel(this.model);
        }
        public void NotifyPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        //Getters
        public ConnectionButtonsViewModel ConnectionButtonsVM {
             get { return ConnectionButtons_vm; }
        }
        public DashboardViewModel DashboardVM
        {
            get { return Dashboard_vm; }
        }
        /*public SlidersViewModel SlidersVM {
            get { return Sliders_vm; }
        }*/
        public JoystickViewModel JoystickVM
        {
            get { return Joystick_vm; }
        }
        public MapViewModel MapVM
        {
            get { return Map_vm; }
        }
        public void Connect(string ip, int port)
        {
            model.Connect(ip, port);
        }
        public void Start()
        {
            model.Start();
        }
    }
}
