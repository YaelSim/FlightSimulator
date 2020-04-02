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

        private double dm_latitude;
        private double dm_longitude;

        public MySimulatorModel(ITelnet tc)
        {
            this.telnetClient = tc;
            this.stop = false;
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
                //ADD ----- NOTIFYPROPERTYCHANGED("RUDDER") TO ALL OF THE PROPERTIES
                // DON'T FORGET TO SEND MESSAGES TO THE SIMULATOR --- COMMAND DP!!!!!!!!!! ****************************
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
            }
        }

        //Determine the plane's location over the Bing map
        public double longitude
        { get { return this.dm_longitude; }
            set
            {
                //?????????????????????
            }
        }
        public double latitude
        {
            get { return this.dm_latitude; }
            set
            {
                //?????????????????????
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
