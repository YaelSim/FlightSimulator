using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using FlightSimulatorApp.Model;
using FlightSimulatorApp.Utilities;
using Microsoft.Maps.MapControl.WPF;

namespace FlightSimulatorApp.ViewModels
{
    public partial class MapViewModel : INotifyPropertyChanged
    {
        private readonly ISimulatorModel model;
        public event PropertyChangedEventHandler PropertyChanged;

        public MapViewModel(ISimulatorModel m)
        {
            model = m;
            model.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e) {
                NotifyPropertyChanged("VM_" + e.PropertyName);
            };
        }
        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        [Obsolete]
        public Location VM_Location
        {
            get
            {
                return model.Location;
            }
        }
        public double VM_Longitude
        {
            get
            {
                return model.Longitude;
            }
        }
        public double VM_Latitude
        {
            get
            {
                return model.Latitude;
            }
        }
    }
}
