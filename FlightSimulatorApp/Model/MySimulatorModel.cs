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
        private readonly ITelnet telnetClient;
        private volatile bool stop;
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly Mutex mutex = new Mutex();

        //Connection Buttons property as a data member
        private string dm_currStatus = "Disconnected";
        private string dm_error;
        private string dm_IPaddress;
        private string dm_port;

        //Dashboard Properties as data members
        private string dm_heading;
        private string dm_verticalSpeed;
        private string dm_groundSpeed;
        private string dm_airSpeed;
        private string dm_altitude;
        private string dm_internalRoll;
        private string dm_internalPitch;
        private string dm_altimeter;

        //Joystick's + Sliders' properties as data members
        private double dm_rudder = 0;
        private double dm_elevator = 0;
        private double dm_aileron = 0;
        private double dm_throttle = 0;

        //Map's properties as data members
        private double dm_latitude = 32.002644;
        private double dm_longitude = 34.888781;
        private Location dm_location = new Location(32.002644, 34.888781);
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
        public string Heading
        {
            get { return dm_heading; }
            set {
                if (Double.TryParse(value, out double temp))
                {
                    //Success in converting to double - No error occurred
                    temp *= 1000;
                    int valAsInt = (int)temp;
                    temp = (double)valAsInt;
                    temp /= 1000;
                    dm_heading = temp.ToString();
                }
                else
                {
                    dm_heading = "ERR";
                }
                NotifyPropertyChanged("Heading");
            }
        }
        public string VerticalSpeed {
            get { return dm_verticalSpeed; }
            set
            {
                if (Double.TryParse(value, out double temp))
                {
                    //Success in converting to double - No error occurred
                    temp *= 1000;
                    int valAsInt = (int)temp;
                    temp = (double)valAsInt;
                    temp /= 1000;
                    dm_verticalSpeed = temp.ToString();
                }
                else
                {
                    dm_verticalSpeed = "ERR";
                }
                NotifyPropertyChanged("VerticalSpeed");
            }
        }
        public string GroundSpeed {
            get { return dm_groundSpeed; }
            set
            {
                if (Double.TryParse(value, out double temp))
                {
                    //Success in converting to double - No error occurred
                    temp *= 1000;
                    int valAsInt = (int)temp;
                    temp = (double)valAsInt;
                    temp /= 1000;
                    dm_groundSpeed = temp.ToString();
                }
                else
                {
                    dm_groundSpeed = "ERR";
                }
                NotifyPropertyChanged("GroundSpeed");
            }
        }
        public string AirSpeed {
            get { return dm_airSpeed; }
            set
            {
                if (Double.TryParse(value, out double temp))
                {
                    //Success in converting to double - No error occurred
                    temp *= 1000;
                    int valAsInt = (int)temp;
                    temp = (double)valAsInt;
                    temp /= 1000;
                    dm_airSpeed = temp.ToString();
                }
                else
                {
                    dm_airSpeed = "ERR";
                }
                NotifyPropertyChanged("AirSpeed");
            }
        }
        public string Altitude {
            get { return dm_altitude; }
            set
            {
                if (Double.TryParse(value, out double temp))
                {
                    //Success in converting to double - No error occurred
                    temp *= 1000;
                    int valAsInt = (int)temp;
                    temp = (double)valAsInt;
                    temp /= 1000;
                    dm_altitude = temp.ToString();
                }
                else
                {
                    dm_altitude = "ERR";
                }
                NotifyPropertyChanged("Altitude");
            }
        }
        public string InternalRoll {
            get { return dm_internalRoll; }
            set
            {
                if (Double.TryParse(value, out double temp))
                {
                    //Success in converting to double - No error occurred
                    temp *= 1000;
                    int valAsInt = (int)temp;
                    temp = (double)valAsInt;
                    temp /= 1000;
                    dm_internalRoll = temp.ToString();
                }
                else
                {
                    dm_internalRoll = "ERR";
                }
                NotifyPropertyChanged("InternalRoll");
            }
        }
        public string InternalPitch {
            get { return dm_internalPitch; }
            set
            {
                if (Double.TryParse(value, out double temp))
                {
                    //Success in converting to double - No error occurred
                    temp *= 1000;
                    int valAsInt = (int)temp;
                    temp = (double)valAsInt;
                    temp /= 1000;
                    dm_internalPitch = temp.ToString();
                }
                else
                {
                    dm_internalPitch = "ERR";
                }
                NotifyPropertyChanged("InternalPitch");
            }
        }
        public string Altimeter {
            get { return dm_altimeter; }
            set
            {
                if (Double.TryParse(value, out double temp))
                {
                    //Success in converting to double - No error occurred
                    temp *= 1000;
                    int valAsInt = (int)temp;
                    temp = (double)valAsInt;
                    temp /= 1000;
                    dm_altimeter = temp.ToString();
                }
                else
                {
                    dm_altimeter = "ERR";
                }

                NotifyPropertyChanged("Altimeter");
            }
        }

        //Joystick's + Sliders' properties
        public double Rudder {
            get { return dm_rudder; }
            set
            {
                double newVal = SetNewValueForRudder(value);
                if (newVal < (-1))
                {
                    newVal = (-1);
                }
                else if (newVal > 1)
                {
                    newVal = 1;
                }
                dm_rudder = newVal;
                NotifyPropertyChanged("Rudder");
            }
        }
        public double Elevator {
            get { return dm_elevator; }
            set
            {
                double newVal = SetNewValueForElevator(value);
                if (newVal < (-1))
                {
                    newVal = (-1);
                }
                else if (newVal > 1)
                {
                    newVal = 1;
                }
                dm_elevator = newVal;
                NotifyPropertyChanged("Elevator");
            }
        }

        public double SetNewValueForRudder(double value)
        {
            double rangeXaml = 250;
            double rangeProperty = 2;
            double proportionalVal = value / rangeXaml;
            double newVal = ((-1) + (proportionalVal * rangeProperty));
            newVal *= 1000;
            int valAsInt = (int)newVal;
            newVal = (double)valAsInt;
            newVal /= 1000;
            return newVal;
        }
        public double SetNewValueForElevator(double value)
        {
            double rangeXaml = 250;
            double rangeProperty = 2;
            double proportionalVal = value / rangeXaml;
            double newVal = (1 - (proportionalVal * rangeProperty));
            newVal *= 1000;
            int valAsInt = (int)newVal;
            newVal = (double)valAsInt;
            newVal /= 1000;
            return newVal;
        }

        public double Aileron {
            get { return dm_aileron; }
            set
            {
                dm_aileron = value;
                NotifyPropertyChanged("Aileron");
            }
        }
        public double Throttle {
            get { return dm_throttle; }
            set
            {
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
                    dm_error = "ERR";
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
                    dm_error = "ERR";
                }
            }
        }
        public Location Location {
            get { return dm_location; }
            set
            {
                dm_location = value;
                NotifyPropertyChanged("Location");
            }
        }
        public void Connect(string ip, int port)
        {
            try
            {
                telnetClient.connect(ip, port);
                CurrStatus = "Connected";
            } catch (TimeoutException)
            {
                //Timeout occurred
                CurrStatus = "Timeout Occurred";
            } catch (Exception)
            {
                CurrStatus = "Connection Has Failed";
            }
        }
        public void Disconnect()
        {
            stop = true;
            CurrStatus = "Disconnected";
            telnetClient.disconnect();
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
                        Heading = tempStr;
                        //think what happens when it IS AN ERROR!!!!!!!!!!!!!!!!!!!!!!!!**************************

                        telnetClient.write("get /instrumentation/gps/indicated-vertical-speed");
                        tempStr = telnetClient.read();
                        VerticalSpeed = tempStr;

                        telnetClient.write("get /instrumentation/gps/indicated-ground-speed-kt");
                        tempStr = telnetClient.read();
                        GroundSpeed = tempStr;

                        telnetClient.write("get /instrumentation/airspeed-indicator/indicated-speed-kt");
                        tempStr = telnetClient.read();
                        AirSpeed = tempStr;

                        telnetClient.write("get /instrumentation/gps/indicated-altitude-ft");
                        tempStr = telnetClient.read();
                        Altitude = tempStr;

                        telnetClient.write("get /instrumentation/attitude-indicator/internal-roll-deg");
                        tempStr = telnetClient.read();
                        InternalRoll = tempStr;

                        telnetClient.write("get /instrumentation/attitude-indicator/internal-pitch-deg");
                        tempStr = telnetClient.read();
                        InternalPitch = tempStr;

                        telnetClient.write("get /instrumentation/altimeter/indicated-altitude-ft");
                        tempStr = telnetClient.read();
                        Altimeter = tempStr;

                        telnetClient.write("get /position/longitude-deg");
                        tempStr = telnetClient.read();
                        if (!(tempStr.Equals("ERR")))
                        {
                            Longitude = Double.Parse(tempStr);
                        }

                        telnetClient.write("get /position/latitude-deg");
                        tempStr = telnetClient.read();
                        if (!(tempStr.Equals("ERR")))
                        {
                            Latitude = Double.Parse(tempStr);
                        }

                        Location = new Location(Latitude, Longitude);

                        Thread.Sleep(250); //SLEEP FOR 250 MS - that determines we will ask for details 4 times in a sec.
                    } catch (Exception e)
                    {
                        Err = "ERR";
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
                string str = telnetClient.read();
                Debug.WriteLine("response: " + str);
                mutex.ReleaseMutex();
            }
        }
        public string Err
        {
            get
            {
                return dm_error;
            }
            set
            {
                dm_error = value;
                NotifyPropertyChanged("Err");
            }
        }
        public string IPaddress { get { return dm_IPaddress; } set { dm_IPaddress = value; } }
        public string Port { get { return dm_port; } set { dm_port = value; } }
    }
}
