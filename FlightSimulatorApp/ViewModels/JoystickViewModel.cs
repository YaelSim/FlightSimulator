using FlightSimulatorApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace FlightSimulatorApp.ViewModels
{
    public partial class JoystickViewModel : INotifyPropertyChanged
    {
        private ISimulatorModel model;
        public event PropertyChangedEventHandler PropertyChanged;

        public JoystickViewModel()
        {
            //this.model = new MySimulatorModel(new TelnetClient());
            this.model = MySimulatorModel.GetSimulatorModel;
            this.model.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e) {
                NotifyPropertyChanged("VM_" + e.PropertyName);
            };
        }
        public void NotifyPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public double VM_aileron
        {
            get
            {
                return this.model.aileron;
            }
            set
            {
                this.model.aileron = value;
            }
        }
        public double VM_rudder
        {
            get
            {
                return this.model.rudder;
            }
            set
            {
                this.model.rudder = value;
            }
        }
        public double VM_elevator
        {
            get
            {
                return this.model.elevator;
            }
            set
            {
                this.model.elevator = value;
            }
        }
        public double VM_throttle
        {
            get
            {
                return this.model.throttle;
            }
            set
            {
                this.model.throttle = value;
            }
        }
    }
}
