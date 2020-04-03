using FlightSimulatorApp.Utilities;
using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;

namespace FlightSimulatorApp.Model
{
    class MySimulatorModel : ISimulatorModel
    {
        //Implementation of singleton to promise that we have exactly ONE SIMULATORMODEL
        private static MySimulatorModel instance = null;
        // DON'T FORGET TO SEND MESSAGES TO THE SIMULATOR --- COMMAND DP!!!!!!!!!! ****************************
        // need to save all of the commands to the simulator in a list/ array / vector of strings. *********************
        private ITelnet telnetClient;
        private volatile bool stop;
        public event PropertyChangedEventHandler PropertyChanged;

        //Dashboard Properties as data members
        private double dm_heading;
        private double dm_verticalSpeed;
        private double dm_groundSpeed;
        private double dm_airSpeed;
        private double dm_altitude;
        private double dm_internalRoll;
        private double dm_internalPitch;
        private double dm_altimeter;

        //Joystick's + Sliders' properties as data members
        private double dm_rudder = 0;
        private double dm_elevator = 0;
        private double dm_aileron = 0;
        private double dm_throttle = 0;

        //Map's properties as data members
        private double dm_latitude = 0;
        private double dm_longitude = 0;
        private Location dm_location = new Location(0, 0);
        private MySimulatorModel(ITelnet tc)
        {
            this.telnetClient = tc;
            this.stop = false;
        }

        //Singleton static method
        public static MySimulatorModel GetSimulatorModel
        {
            get {
                if (instance != null)
                {
                    return instance;
                } else
                {
                    TelnetClient tc = new TelnetClient();
                    MySimulatorModel.instance = new MySimulatorModel(tc);
                    return instance;
                }
            }
        }

        //Dashboard Properties
        public double heading
        {
            get { return this.dm_heading; }
            set {
                this.dm_heading = value;
                NotifyPropertyChanged("heading");
            }
        }
        public double verticalSpeed {
            get { return this.dm_verticalSpeed; }
            set
            {
                this.dm_verticalSpeed = value;
                NotifyPropertyChanged("verticalSpeed");
            }
        }
        public double groundSpeed {
            get { return this.dm_groundSpeed; }
            set
            {
                this.dm_groundSpeed = value;
                NotifyPropertyChanged("groundSpeed");
            }
        }
        public double airSpeed {
            get { return this.dm_airSpeed; }
            set
            {
                this.dm_airSpeed = value;
                NotifyPropertyChanged("airSpeed");
            }
        }
        public double altitude {
            get { return this.dm_altitude; }
            set
            {
                this.dm_altitude = value;
                NotifyPropertyChanged("altitude");
            }
        }
        public double internalRoll {
            get { return this.dm_internalRoll; }
            set
            {
                this.dm_internalRoll = value;
                NotifyPropertyChanged("internalRoll");
            }
        }
        public double internalPitch {
            get { return this.dm_internalPitch; }
            set
            {
                this.dm_internalPitch = value;
                NotifyPropertyChanged("internalPitch");
            }
        }
        public double altimeter {
            get { return this.dm_altimeter; }
            set
            {
                this.dm_altimeter = value;
                NotifyPropertyChanged("altimeter");
            }
        }

        //Joystick's + Sliders' properties
        public double rudder {
            get { return this.dm_rudder; }
            set
            {
                if (value < (-1))
                {
                    value = (-1);
                }
                else if (value > 1)
                {
                    value = 1;
                }
                this.dm_rudder = value;
                NotifyPropertyChanged("rudder");
            }
        }
        public double elevator {
            get { return this.dm_elevator; }
            set
            {
                if (value < (-1))
                {
                    value = (-1);
                }
                else if (value > 1)
                {
                    value = 1;
                }
                this.dm_elevator = value;
                NotifyPropertyChanged("elevator");
            }
        }
        public double aileron {
            get { return this.dm_aileron; }
            set
            {
                if (value < (-1))
                {
                    value = (-1);
                }
                else if (value > 1)
                {
                    value = 1;
                }
                this.dm_aileron = value;
                NotifyPropertyChanged("aileron");
            }
        }
        public double throttle {
            get { return this.dm_throttle; }
            set
            {
                if (value < 0)
                {
                    value = 0;
                }
                else if (value > 1)
                {
                    value = 1;
                }
                this.dm_throttle = value;
                NotifyPropertyChanged("throttle");
            }
        }

        //Map's properties - Determine the plane's location over the Bing map
        //VERIFY if we did it correct *******************************
        public double longitude
        { get { return this.dm_longitude; }
            set
            {
                this.dm_longitude = value;
                NotifyPropertyChanged("longitude");
            }
        }
        public double latitude
        {
            get { return this.dm_latitude; }
            set
            {
                this.dm_latitude = value;
                NotifyPropertyChanged("latitude");
            }
        }
        public Location location {
            get { return this.dm_location; }
            set
            {
                NotifyPropertyChanged("location");
            }
        }
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
                    string tempStr;
                    this.telnetClient.write("get /instrumentation/heading-indicator/indicated-heading-deg");
                    tempStr = this.telnetClient.read();
                    if (!(tempStr.Equals("ERR")))
                    {
                        heading = Double.Parse(tempStr);
                    }
                    //think what happens when it IS AN ERROR!!!!!!!!!!!!!!!!!!!!!!!!**************************

                    this.telnetClient.write("get /instrumentation/gps/indicated-vertical-speed");
                    tempStr = this.telnetClient.read();
                    if (!(tempStr.Equals("ERR")))
                    {
                        verticalSpeed = Double.Parse(tempStr);
                    }

                    this.telnetClient.write("get /instrumentation/gps/indicated-ground-speed-kt");
                    tempStr = this.telnetClient.read();
                    if (!(tempStr.Equals("ERR")))
                    {
                        groundSpeed = Double.Parse(tempStr);
                    }

                    this.telnetClient.write("get /instrumentation/airspeed-indicator/indicated-speed-kt");
                    tempStr = this.telnetClient.read();
                    if (!(tempStr.Equals("ERR")))
                    {
                        airSpeed = Double.Parse(tempStr);
                    }

                    this.telnetClient.write("get /instrumentation/gps/indicated-altitude-ft");
                    tempStr = this.telnetClient.read();
                    if (!(tempStr.Equals("ERR")))
                    {
                        altitude = Double.Parse(tempStr);
                    }

                    this.telnetClient.write("get /instrumentation/attitude-indicator/internal-roll-deg");
                    tempStr = this.telnetClient.read();
                    if (!(tempStr.Equals("ERR")))
                    {
                        internalRoll = Double.Parse(tempStr);
                    }

                    this.telnetClient.write("get /instrumentation/attitude-indicator/internal-pitch-deg");
                    tempStr = this.telnetClient.read();
                    if (!(tempStr.Equals("ERR")))
                    {
                        internalPitch = Double.Parse(tempStr);
                    }
                    
                    this.telnetClient.write("get /instrumentation/altimeter/indicated-altitude-ft");
                    tempStr = this.telnetClient.read();
                    if (!(tempStr.Equals("ERR")))
                    {
                        altimeter = Double.Parse(tempStr);
                    }

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
