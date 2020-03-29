using FlightSimulatorApp.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;

namespace FlightSimulatorApp.Model
{
    class MySimulatorModel : ISimulatorModel
    {
        private ITelnet telnetClient;
        private volatile bool stop;
        public MySimulatorModel(ITelnet tc)
        {
            this.telnetClient = tc;
            this.stop = false;
        }

        public double heading { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double verticalSpeed { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double groundSpeed { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double airSpeed { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double altitude { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double internalRoll { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double internalPitch { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double altimeter { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public event PropertyChangedEventHandler PropertyChanged;

        public void connect(string ip, int port)
        {
            this.telnetClient.connect(ip, port);
        }

        public void disconnect()
        {
            this.stop = true;
            this.telnetClient.disconnect();
        }

        public void start()
        {
            new Thread(delegate ()
            {
                while (!stop)
                {
                    // for example - COMPLETE FOR ALL PROPERTIES!!!!!
                    this.telnetClient.write("get airSpeed property ");
                    airSpeed = Double.Parse(this.telnetClient.read());
                    // The same for the other properties

                    Thread.Sleep(250); //SLEEP FOR 250 MS - that determines we will ask for details 4 times in a sec.

                }
            }).Start();
        }

        public void NotifyPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
