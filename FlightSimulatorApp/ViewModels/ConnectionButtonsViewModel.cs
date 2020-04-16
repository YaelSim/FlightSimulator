using FlightSimulatorApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace FlightSimulatorApp.ViewModels
{
    public partial class ConnectionButtonsViewModel : INotifyPropertyChanged
    {
        private readonly ISimulatorModel model;
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
        public void Connect(string ip, string port)
        {
            model.Connect(ip, port);
        }
        public void Disconnect()
        {
            model.Disconnect();
        }
        public string VM_CurrStatus { get { return model.CurrStatus; }
            set
            {
                model.CurrStatus = value;
                NotifyPropertyChanged("VM_CurrStatus");
            }
        }
        public string VM_IPaddress
        {
            get { return model.IPaddress; }
            set
            {
                model.IPaddress = value;
                NotifyPropertyChanged("VM_IPaddress");
            }
        }
        public string VM_Port
        {
            get { return model.Port; }
            set
            {
                model.Port = value;
                NotifyPropertyChanged("VM_Port");
            }
        }
        public string VM_Err
        {
            get { return model.Err; }
            set
            {
                model.Err = value;
                NotifyPropertyChanged("VM_Err");
            }
        }
    }
}
