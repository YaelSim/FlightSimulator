using FlightSimulatorApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace FlightSimulatorApp.ViewModels
{
    public partial class FlightControlsViewModel : INotifyPropertyChanged
    {
        private ISimulatorModel model;
        public event PropertyChangedEventHandler PropertyChanged;
        public FlightControlsViewModel(ISimulatorModel m)
        {
            this.model = m;
            this.model.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e) {
                NotifyPropertyChanged("VM_" + e.PropertyName);
            };
        }
        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public void SendCommandToModel(string cmd)
        {
            this.model.SendCommandToSimulator(cmd);
        }
        //Joystick Properties
        public double VM_Rudder
        {
            get
            {
                return this.model.Rudder;
            }
            set
            {
                if (value < (-1))
                {
                    value = (-1);
                }
                else if (value > 1)
                {
                    value = 1;
                }
                this.model.Rudder = value;
                //Convert the value (double) to string, in order to pass it to the ViewModel.
                string valAsString = value.ToString();
                string cmd = "set /controls/flight/rudder " + valAsString;
                SendCommandToModel(cmd);
                NotifyPropertyChanged("VM_Rudder");
            }
        }
        public double VM_Elevator
        {
            get
            {
                return this.model.Elevator;
            }
            set
            {
                if (value < (-1))
                {
                    value = (-1);
                }
                else if (value > 1)
                {
                    value = 1;
                }
                this.model.Elevator = value;
                //Convert the value (double) to string, in order to pass it to the ViewModel.
                string valAsString = value.ToString();
                string cmd = "set /controls/flight/elevator " + valAsString;
                SendCommandToModel(cmd);
                NotifyPropertyChanged("VM_Elevator");
            }
        }
        //Sliders' Properties
        public double VM_Aileron
        {
            get
            {
                return this.model.Aileron;
            }
            set
            {
                if (value < (-1))
                {
                    value = (-1);
                }
                else if (value > 1)
                {
                    value = 1;
                }
                this.model.Aileron = value;
                NotifyPropertyChanged("VM_Aileron");
            }
        }
        public double VM_Throttle
        {
            get
            {
                return this.model.Throttle;
            }
            set
            {
                if (value < 0)
                {
                    value = 0;
                }
                else if (value > 1)
                {
                    value = 1;
                }
                this.model.Throttle = value;
                NotifyPropertyChanged("VM_Throttle");
            }
        }
    }
}
