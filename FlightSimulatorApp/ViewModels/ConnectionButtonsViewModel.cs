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
        public ConnectionButtonsViewModel(ISimulatorModel m) {
            model = m;
            model.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e) {
                NotifyPropertyChanged("VM_" + e.PropertyName);
            };
        }
        public void NotifyPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public void SendCommandToModel(string cmd)
        {
            this.model.SendCommandToSimulator(cmd);
        }
        public void Connect(string ip, int port)
        {
            model.Connect(ip, port);
        }
        public void Disconnect()
        {
            model.Disconnect();
        }
        public string VM_CurrStatus { get { return this.model.CurrStatus; } }
        public string IPaddress
        {
            get { return model.IPaddress; }
            set
            {
                model.IPaddress = value;
                NotifyPropertyChanged("IPaddress");
            }
        }
        public int Port
        {
            get { return model.Port; }
            set
            {
                model.Port = value;
                NotifyPropertyChanged("Port");
            }
        }
    }
}
