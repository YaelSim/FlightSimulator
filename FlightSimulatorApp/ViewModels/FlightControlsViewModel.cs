using FlightSimulatorApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;

namespace FlightSimulatorApp.ViewModels
{
    public partial class FlightControlsViewModel : INotifyPropertyChanged
    {
        private readonly ISimulatorModel model;
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
            model.SendCommandToSimulator(cmd);
        }
        //Joystick Properties and help methods
        public double VM_Rudder
        {
            get
            {
                return model.Rudder;
            }
            set
            {
                double newVal = SetNewValueForRudder(value);
                if (newVal < (-1))
                {
                    newVal = (-1);
                }
                else if (newVal > 1)
                {
                    newVal = 1;
                }
                model.Rudder = newVal;
                //Convert the value (double) to string, in order to pass it to the ViewModel.
                string valAsString = newVal.ToString();
                string cmd = "set /controls/flight/rudder " + valAsString;
                SendCommandToModel(cmd);
                NotifyPropertyChanged("VM_Rudder");
            }
        }
        public double VM_Elevator
        {
            get
            {
                return model.Elevator;
            }
            set
            {
                double newVal = SetNewValueForElevator(value);
                if (newVal < (-1))
                {
                    newVal = (-1);
                }
                else if (newVal > 1)
                {
                    newVal = 1;
                }
                model.Elevator = newVal;

                //Convert the value (double) to string, in order to pass it to the ViewModel.
                string valAsString = newVal.ToString();
                string cmd = "set /controls/flight/elevator " + valAsString;
                SendCommandToModel(cmd);
                NotifyPropertyChanged("VM_Elevator");
            }
        }
        public double SetNewValueForRudder(double value)
        {
            double rangeXaml = 250;
            double rangeProperty = 2;
            double proportionalVal = value / rangeXaml;
            double newVal = ((-1) + (proportionalVal * rangeProperty));
            newVal *= 1000;
            int valAsInt = (int)newVal;
            newVal = (double)valAsInt;
            newVal /= 1000;
            return newVal;
        }
        public double SetNewValueForElevator(double value)
        {
            double rangeXaml = 250;
            double rangeProperty = 2;
            double proportionalVal = value / rangeXaml;
            double newVal = (1 - (proportionalVal * rangeProperty));
            newVal *= 1000;
            int valAsInt = (int)newVal;
            newVal = (double)valAsInt;
            newVal /= 1000;
            return newVal;
        }
        public double VM_X
        {
            get {
                double value = VM_Rudder;
                double rangeXaml = 250;
                double rangeProperty = 2;
                double proportionalVal = (value + 1) / rangeProperty;
                double newVal = proportionalVal * rangeXaml;
                return newVal;
            }
            set { VM_Rudder = value; }
        }
        public double VM_Y
        {
            get {
                double value = VM_Elevator;
                double rangeXaml = 250;
                double rangeProperty = 2;
                double proportionalVal = (1 - value) / rangeProperty;
                double newVal = proportionalVal * rangeXaml;
                return newVal;
            }
            set { VM_Elevator = value; }
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
