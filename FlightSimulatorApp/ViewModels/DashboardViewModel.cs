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

        public DashboardViewModel(ISimulatorModel m)
        {
            //this.model = MySimulatorModel.GetSimulatorModel;
            this.model = m;
            this.model.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e) {
                NotifyPropertyChanged("VM_" + e.PropertyName);
            };
        }
        public void NotifyPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public double VM_Heading
        {
            get
            {
                return this.model.Heading;
            }
        }
        public double VM_VerticalSpeed
        {
            get
            {
                return this.model.VerticalSpeed;
            }
        }
        public double VM_GroundSpeed
        {
            get
            {
                return this.model.GroundSpeed;
            }
        }
        public double VM_AirSpeed
        {
            get
            {
                return this.model.AirSpeed;
            }
        }
        public double VM_Altitude
        {
            get
            {
                return this.model.Altitude;
            }
        }
        public double VM_InternalRoll
        {
            get
            {
                return this.model.InternalRoll;
            }
        }
        public double VM_InternalPitch
        {
            get
            {
                return this.model.InternalPitch;
            }
        }
        public double VM_Altimeter
        {
            get
            {
                return this.model.Altimeter;
            }
        }
    }
}
