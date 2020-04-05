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
        private ISimulatorModel model;
        public event PropertyChangedEventHandler PropertyChanged;

        public MapViewModel()
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
        [Obsolete]
        public Location VM_Location
        {
            get
            {
                Console.WriteLine("location" + model.Latitude + "," + model.Longitude);
                return this.model.Location;
            }
        }
        public double VM_Longitude
        {
            get
            {
                return this.model.Longitude;
            }
        }
        public double VM_Latitude
        {
            get
            {
                return this.model.Latitude;
            }
        }
    }
}
