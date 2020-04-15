using FlightSimulatorApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace FlightSimulatorApp.ViewModels
{
    public partial class DashboardViewModel : INotifyPropertyChanged
    {
        private readonly ISimulatorModel model;
        public event PropertyChangedEventHandler PropertyChanged;

        public DashboardViewModel(ISimulatorModel m)
        {
            this.model = m;
            this.model.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e) {
                NotifyPropertyChanged("VM_" + e.PropertyName);
            };
        }
        public void NotifyPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public string VM_Heading
        {
            get
            {
                return model.Heading;
            }
        }
        public string VM_VerticalSpeed
        {
            get
            {
                return model.VerticalSpeed;
            }
        }
        public string VM_GroundSpeed
        {
            get
            {
                return model.GroundSpeed;
            }
        }
        public string VM_AirSpeed
        {
            get
            {
                return model.AirSpeed;
            }
        }
        public string VM_Altitude
        {
            get
            {
                return model.Altitude;
            }
        }
        public string VM_InternalRoll
        {
            get
            {
                return model.InternalRoll;
            }
        }
        public string VM_InternalPitch
        {
            get
            {
                return model.InternalPitch;
            }
        }
        public string VM_Altimeter
        {
            get
            {
                return model.Altimeter;
            }
        }
    }
}
