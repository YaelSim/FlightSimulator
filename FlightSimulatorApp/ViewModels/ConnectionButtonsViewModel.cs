using FlightSimulatorApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace FlightSimulatorApp.ViewModels
{
    public partial class ConnectionButtonsViewModel : INotifyPropertyChanged
    {
        private ISimulatorModel model;
        public event PropertyChangedEventHandler PropertyChanged;
        public ConnectionButtonsViewModel() {
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

        public string VM_CurrStatus { get { return this.model.CurrStatus; } }
    }
}
