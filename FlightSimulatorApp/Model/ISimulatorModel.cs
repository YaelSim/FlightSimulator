using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Microsoft.Maps.MapControl.WPF;

namespace FlightSimulatorApp.Model
{
    interface ISimulatorModel : INotifyPropertyChanged
    {
        //Methods that are responsible for connecting to the simulator
        void Connect(string ip, int port);
        void Disconnect();
        void Start();
        void SendCommandToSimulator(string command);

        //Simulator Properties
        double Heading { get; set; }
        double VerticalSpeed { get; set; }
        double GroundSpeed { get; set; }
        double AirSpeed { get; set; }
        double Altitude { get; set; }
        double InternalRoll { get; set; }
        double InternalPitch { get; set; }
        double Altimeter { get; set; }

        //Activate actuators
        double Rudder { get; set; }
        double Elevator { get; set; }
        double Aileron { get; set; }
        double Throttle { get; set; }

        //Map's properties
        double Longitude { get; set; }
        double Latitude { get; set; }
        Location Location { get; set; }

        //Connection buttons' property
        string CurrStatus { get; set; }
    }
}
