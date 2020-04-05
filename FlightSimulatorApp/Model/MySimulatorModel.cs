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

        //Connection Buttons property as a data member
        private string dm_currStatus = "Disconnected";

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

        //Connection Buttons property
        public string CurrStatus
        {
            get { return this.dm_currStatus; }
            set
            {
                this.dm_currStatus = value;
            }
        }

        //Dashboard Properties
        public double Heading
        {
            get { return this.dm_heading; }
            set {
                this.dm_heading = value;
                NotifyPropertyChanged("heading");
            }
        }
        public double VerticalSpeed {
            get { return this.dm_verticalSpeed; }
            set
            {
                this.dm_verticalSpeed = value;
                NotifyPropertyChanged("verticalSpeed");
            }
        }
        public double GroundSpeed {
            get { return this.dm_groundSpeed; }
            set
            {
                this.dm_groundSpeed = value;
                NotifyPropertyChanged("groundSpeed");
            }
        }
        public double AirSpeed {
            get { return this.dm_airSpeed; }
            set
            {
                this.dm_airSpeed = value;
                NotifyPropertyChanged("airSpeed");
            }
        }
        public double Altitude {
            get { return this.dm_altitude; }
            set
            {
                this.dm_altitude = value;
                NotifyPropertyChanged("altitude");
            }
        }
        public double InternalRoll {
            get { return this.dm_internalRoll; }
            set
            {
                this.dm_internalRoll = value;
                NotifyPropertyChanged("internalRoll");
            }
        }
        public double InternalPitch {
            get { return this.dm_internalPitch; }
            set
            {
                this.dm_internalPitch = value;
                NotifyPropertyChanged("internalPitch");
            }
        }
        public double Altimeter {
            get { return this.dm_altimeter; }
            set
            {
                this.dm_altimeter = value;
                NotifyPropertyChanged("altimeter");
            }
        }

        //Joystick's + Sliders' properties
        public double Rudder {
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
        public double Elevator {
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
        public double Aileron {
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
        public double Throttle {
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
        public double Longitude
        { get { return this.dm_longitude; }
            set
            {
                this.dm_longitude = value;
                NotifyPropertyChanged("longitude");
            }
        }
        public double Latitude
        {
            get { return this.dm_latitude; }
            set
            {
                this.dm_latitude = value;
                NotifyPropertyChanged("latitude");
            }
        }
        public Location Location {
            get { return this.dm_location; }
            set
            {
                NotifyPropertyChanged("location");
            }
        }
        public void Connect(string ip, int port)
        {
            this.telnetClient.connect(ip, port);
        }
        public void Disconnect()
        {
            this.stop = true;
            this.telnetClient.disconnect();
        }
        public void Start()
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
                        Heading = Double.Parse(tempStr);
                    }
                    //think what happens when it IS AN ERROR!!!!!!!!!!!!!!!!!!!!!!!!**************************

                    this.telnetClient.write("get /instrumentation/gps/indicated-vertical-speed");
                    tempStr = this.telnetClient.read();
                    if (!(tempStr.Equals("ERR")))
                    {
                        VerticalSpeed = Double.Parse(tempStr);
                    }

                    this.telnetClient.write("get /instrumentation/gps/indicated-ground-speed-kt");
                    tempStr = this.telnetClient.read();
                    if (!(tempStr.Equals("ERR")))
                    {
                        GroundSpeed = Double.Parse(tempStr);
                    }

                    this.telnetClient.write("get /instrumentation/airspeed-indicator/indicated-speed-kt");
                    tempStr = this.telnetClient.read();
                    if (!(tempStr.Equals("ERR")))
                    {
                        AirSpeed = Double.Parse(tempStr);
                    }

                    this.telnetClient.write("get /instrumentation/gps/indicated-altitude-ft");
                    tempStr = this.telnetClient.read();
                    if (!(tempStr.Equals("ERR")))
                    {
                        Altitude = Double.Parse(tempStr);
                    }

                    this.telnetClient.write("get /instrumentation/attitude-indicator/internal-roll-deg");
                    tempStr = this.telnetClient.read();
                    if (!(tempStr.Equals("ERR")))
                    {
                        InternalRoll = Double.Parse(tempStr);
                    }

                    this.telnetClient.write("get /instrumentation/attitude-indicator/internal-pitch-deg");
                    tempStr = this.telnetClient.read();
                    if (!(tempStr.Equals("ERR")))
                    {
                        InternalPitch = Double.Parse(tempStr);
                    }
                    
                    this.telnetClient.write("get /instrumentation/altimeter/indicated-altitude-ft");
                    tempStr = this.telnetClient.read();
                    if (!(tempStr.Equals("ERR")))
                    {
                        Altimeter = Double.Parse(tempStr);
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
