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
            this.model = new MySimulatorModel(new TelnetClient());
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
        public Location VM_location
        {
            get
            {
                Console.WriteLine("location" + model.latitude + "," + model.longitude);
                return this.model.location;
            }
        }

        public double VM_longitude
        {
            get
            {
                return this.model.longitude;
            }
        }

        public double VM_latitude
        {
            get
            {
                return this.model.latitude;
            }
        }
    }
}
