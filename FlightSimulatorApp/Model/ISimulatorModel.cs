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
        void connect(string ip, int port);
        void disconnect();
        void start();

        //Simulator Properties
        double heading { get; set; }
        double verticalSpeed { get; set; }
        double groundSpeed { get; set; }
        double airSpeed { get; set; }
        double altitude { get; set; }
        double internalRoll { get; set; }
        double internalPitch { get; set; }
        double altimeter { get; set; }

        //Activate actuators
        double rudder { get; set; }
        double elevator { get; set; }
        double aileron { get; set; }
        double throttle { get; set; }

        //Map's properties
        double longitude { get; set; }
        double latitude { get; set; }
        Location location { get; set; }
    }
}
