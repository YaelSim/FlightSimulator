using FlightSimulatorApp.Utilities;
using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace FlightSimulatorApp.Model
{
    public class MySimulatorModel : ISimulatorModel
    {
        //Implementation of singleton to promise that we have exactly ONE SIMULATORMODEL
        //private static MySimulatorModel instance = null;
        private ITelnet telnetClient;
        private volatile bool stop;
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly Mutex mutex = new Mutex();

        //Connection Buttons property as a data member
        private string dm_currStatus = "Disconnected";
        private bool dm_error = false;
        private string dm_IPaddress;
        private int dm_port;

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
        public MySimulatorModel(ITelnet tc)
        {
            this.telnetClient = tc;
            this.stop = false;
        }

        //Connection Buttons property
        public string CurrStatus
        {
            get { return dm_currStatus; }
            set
            {
                dm_currStatus = value;
                NotifyPropertyChanged("CurrStatus");
            }
        }

        //Dashboard Properties
        public double Heading
        {
            get { return dm_heading; }
            set {
                dm_heading = value;
                NotifyPropertyChanged("Heading");
            }
        }
        public double VerticalSpeed {
            get { return dm_verticalSpeed; }
            set
            {
                dm_verticalSpeed = value;
                NotifyPropertyChanged("VerticalSpeed");
            }
        }
        public double GroundSpeed {
            get { return dm_groundSpeed; }
            set
            {
                dm_groundSpeed = value;
                NotifyPropertyChanged("GroundSpeed");
            }
        }
        public double AirSpeed {
            get { return dm_airSpeed; }
            set
            {
                dm_airSpeed = value;
                NotifyPropertyChanged("AirSpeed");
            }
        }
        public double Altitude {
            get { return dm_altitude; }
            set
            {
                dm_altitude = value;
                NotifyPropertyChanged("Altitude");
            }
        }
        public double InternalRoll {
            get { return dm_internalRoll; }
            set
            {
                dm_internalRoll = value;
                NotifyPropertyChanged("InternalRoll");
            }
        }
        public double InternalPitch {
            get { return dm_internalPitch; }
            set
            {
                dm_internalPitch = value;
                NotifyPropertyChanged("InternalPitch");
            }
        }
        public double Altimeter {
            get { return dm_altimeter; }
            set
            {
                dm_altimeter = value;
                NotifyPropertyChanged("Altimeter");
            }
        }

        //Joystick's + Sliders' properties
        public double Rudder {
            get { return dm_rudder; }
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
                dm_rudder = value;
                NotifyPropertyChanged("Rudder");
            }
        }
        public double Elevator {
            get { return dm_elevator; }
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
                dm_elevator = value;
                NotifyPropertyChanged("Elevator");
            }
        }
        public double Aileron {
            get { return dm_aileron; }
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
                dm_aileron = value;
                NotifyPropertyChanged("Aileron");
            }
        }
        public double Throttle {
            get { return dm_throttle; }
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
                dm_throttle = value;
                NotifyPropertyChanged("Throttle");
            }
        }

        //Map's properties - Determine the plane's location over the Bing map
        public double Longitude
        { get { return dm_longitude; }
            set
            {
                if ((value >= -180) && (value <= 180))
                {
                    dm_longitude = value;
                    NotifyPropertyChanged("Longitude");
                } else
                {
                    dm_error = true;
                }
            }
        }
        public double Latitude
        {
            get { return dm_latitude; }
            set
            {
                if ((value <= 90) && (value >= -90))
                {
                    dm_latitude = value;
                    NotifyPropertyChanged("Latitude");
                } else
                {
                    dm_error = true;
                }
            }
        }
        public Location Location {
            get { return dm_location; }
            set
            {
                NotifyPropertyChanged("Location");
            }
        }
        public void Connect(string ip, int port)
        {
            telnetClient.connect(ip, port);
            CurrStatus = "Connected";
        }
        public void Disconnect()
        {
            stop = true;
            telnetClient.disconnect();
            CurrStatus = "Disconnected";
        }
        public void Start()
        {
            new Thread(delegate ()
            {
                while (!stop)
                {
                    try
                    {
                        string tempStr;
                        telnetClient.write("get /instrumentation/heading-indicator/indicated-heading-deg");
                        tempStr = telnetClient.read();
                        if (!(tempStr.Equals("ERR")))
                        {
                            Heading = Double.Parse(tempStr);
                        }
                        //think what happens when it IS AN ERROR!!!!!!!!!!!!!!!!!!!!!!!!**************************

                        telnetClient.write("get /instrumentation/gps/indicated-vertical-speed");
                        tempStr = telnetClient.read();
                        if (!(tempStr.Equals("ERR")))
                        {
                            VerticalSpeed = Double.Parse(tempStr);
                        }

                        telnetClient.write("get /instrumentation/gps/indicated-ground-speed-kt");
                        tempStr = telnetClient.read();
                        if (!(tempStr.Equals("ERR")))
                        {
                            GroundSpeed = Double.Parse(tempStr);
                        }

                        telnetClient.write("get /instrumentation/airspeed-indicator/indicated-speed-kt");
                        tempStr = telnetClient.read();
                        if (!(tempStr.Equals("ERR")))
                        {
                            AirSpeed = Double.Parse(tempStr);
                        }

                        telnetClient.write("get /instrumentation/gps/indicated-altitude-ft");
                        tempStr = telnetClient.read();
                        if (!(tempStr.Equals("ERR")))
                        {
                            Altitude = Double.Parse(tempStr);
                        }

                        telnetClient.write("get /instrumentation/attitude-indicator/internal-roll-deg");
                        tempStr = telnetClient.read();
                        if (!(tempStr.Equals("ERR")))
                        {
                            InternalRoll = Double.Parse(tempStr);
                        }

                        telnetClient.write("get /instrumentation/attitude-indicator/internal-pitch-deg");
                        tempStr = telnetClient.read();
                        if (!(tempStr.Equals("ERR")))
                        {
                            InternalPitch = Double.Parse(tempStr);
                        }

                        telnetClient.write("get /instrumentation/altimeter/indicated-altitude-ft");
                        tempStr = telnetClient.read();
                        if (!(tempStr.Equals("ERR")))
                        {
                            Altimeter = Double.Parse(tempStr);
                        }

                        Thread.Sleep(250); //SLEEP FOR 250 MS - that determines we will ask for details 4 times in a sec.
                    } catch (Exception e)
                    {
                        Err = true;
                        CurrStatus = e.ToString();
                        Thread.Sleep(250);
                    }
                }
            }).Start();
        }
        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public void SendCommandToSimulator(string command)
        {
            if (!stop)
            {
                mutex.WaitOne();
                telnetClient.write(command);
                Debug.WriteLine("sent: " + command);
                // DELETE AFTERWARDS ************************************************************
                string str = this.telnetClient.read();
                Debug.WriteLine("response: " + str);
                mutex.ReleaseMutex();
            }
        }
        public bool Err
        {
            get
            {
                return dm_error;
            }
            set
            {
                if (!dm_error && value)
                {
                    dm_error = value;
                    NotifyPropertyChanged("Err");
                }
                else
                {
                    if (dm_error && !value)
                    {
                        dm_error = value;
                    }
                }
            }
        }
        // CHECK IF NOTIFYPROPERTYCHANGED is needed **************************************************************
        public string IPaddress { get { return dm_IPaddress; } set { dm_IPaddress = value; } }
        public int Port { get { return dm_port; } set { dm_port = value; } }
    }
}
