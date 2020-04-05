using FlightSimulatorApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace FlightSimulatorApp.ViewModels
{
    public partial class DashboardViewModel : INotifyPropertyChanged
    {
        private ISimulatorModel model;
        public event PropertyChangedEventHandler PropertyChanged;

        public DashboardViewModel()
        {
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
        public double VM_heading
        {
            get
            {
                return this.model.heading;
            }
        }
        public double VM_verticalSpeed
        {
            get
            {
                return this.model.verticalSpeed;
            }
        }
        public double VM_groundSpeed
        {
            get
            {
                return this.model.groundSpeed;
            }
        }
        public double VM_airSpeed
        {
            get
            {
                return this.model.airSpeed;
            }
        }
        public double VM_altitude
        {
            get
            {
                return this.model.altitude;
            }
        }
        public double VM_internalRoll
        {
            get
            {
                return this.model.internalRoll;
            }
        }
        public double VM_internalPitch
        {
            get
            {
                return this.model.internalPitch;
            }
        }
        public double VM_altimeter
        {
            get
            {
                return this.model.altimeter;
            }
        }
    }
}
