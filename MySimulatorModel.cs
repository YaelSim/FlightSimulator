using FlightSimulatorApp.Utilities;
using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
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
        private volatile bool errorsInIpAndPort = false;
        private readonly Queue<string> commandsQueue = new Queue<string>();
        //Connection Buttons property as a data member
        private string dm_currStatus = "Disconnected";
        private string dm_error;
        [Obsolete]
        private string dm_IPaddress = ConfigurationSettings.AppSettings.Get("IPdefault");
        [Obsolete]
        private string dm_port = ConfigurationSettings.AppSettings.Get("Portdefault");

        //Dashboard Properties as data members
        private string dm_heading = "ERR";
        private string dm_verticalSpeed = "ERR";
        private string dm_groundSpeed = "ERR";
        private string dm_airSpeed = "ERR";
        private string dm_altitude = "ERR";
        private string dm_internalRoll = "ERR";
        private string dm_internalPitch = "ERR";
        private string dm_altimeter = "ERR";

        //Joystick's + Sliders' properties as data members
        private double dm_rudder = 0;
        private double dm_elevator = 0;
        private double dm_aileron = 0;
        private double dm_throttle = 0;

        //Map's properties as data members
        private double dm_latitude = 32.002644;
        private double dm_longitude = 34.888781;
        private Location dm_location = new Location(32.002644, 34.888781);
        private readonly Mutex mutex = new Mutex();
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
                dm_rudder = value;
                NotifyPropertyChanged("Rudder");
            }
        }
        public double Elevator {
            get { return dm_elevator; }
            set
            {
                dm_elevator = value;
                NotifyPropertyChanged("Elevator");
            }
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
        public void Connect(string ip, string port)
        {
            try
            {
                //Checking if the given ip and port are valid!
                if (Int32.TryParse(port, out int portAsInt))
                {
                    if ((ip.Contains(".")) && (System.Net.IPAddress.TryParse(ip, out System.Net.IPAddress address)))
                    {
                        try
                        {
                            telnetClient.Connect(ip, portAsInt);
                            stop = false;
                            CurrStatus = "Connected";
                            errorsInIpAndPort = false;
                            Err = "";
                        } catch (Exception)
                        {
                            CurrStatus = "Connection Has Failed";
                        }
                    } else
                    {
                        CurrStatus = "Invalid port or IP address - Please try again";
                        errorsInIpAndPort = true;
                    }
                } else
                {
                    CurrStatus = "Invalid port or IP address - Please try again";
                    errorsInIpAndPort = true;
                }
            }
            catch (TimeoutException)
            {
                //Timeout occurred
                CurrStatus = "Timeout Occurred";
            }
            catch (Exception)
            {
                CurrStatus = "Connection Has Failed";
            }
        }
        public void Disconnect()
        {
            try
            {
                if (!errorsInIpAndPort)
                {
                    stop = true;
                    telnetClient.Disconnect();
                    CurrStatus = "Disconnected";
                    Err = "";
                }
            } catch (Exception)
            {
                stop = true;
                CurrStatus = "Disconnected";
                Err = "";
            }
            
        }
        public void Start()
        {
            if (!errorsInIpAndPort)
            {
                new Thread(delegate ()
                {
                    while (!stop)
                    {
                        mutex.WaitOne();
                        try
                        {
                            string tempStr;
                            if (telnetClient.IsSocketAvailableWriting())
                            {
                                telnetClient.Write("get /instrumentation/heading-indicator/indicated-heading-deg");
                            }
                            else
                            {
                                Err = "Timeout - Heading";
                            }
                            if (telnetClient.IsSocketAvailableReading())
                            {
                                tempStr = telnetClient.Read();
                                Heading = tempStr;
                                if (Err.Equals("Timeout - Heading"))
                                {
                                    Err = "";
                                }
                            }
                            else
                            {
                                Err = "Timeout - Heading";
                            }

                            if (telnetClient.IsSocketAvailableWriting())
                            {
                                telnetClient.Write("get /instrumentation/gps/indicated-vertical-speed");
                            }
                            else
                            {
                                Err = "Timeout - VerticalSpeed";
                            }
                            if (telnetClient.IsSocketAvailableReading())
                            {
                                tempStr = telnetClient.Read();
                                VerticalSpeed = tempStr;
                                if (Err.Equals("Timeout - VerticalSpeed"))
                                {
                                    Err = "";
                                }
                            }
                            else
                            {
                                Err = "Timeout - VerticalSpeed";
                            }

                            if (telnetClient.IsSocketAvailableWriting())
                            {
                                telnetClient.Write("get /instrumentation/gps/indicated-ground-speed-kt");
                            }
                            else
                            {
                                Err = "Timeout - GroundSpeed";
                            }
                            if (telnetClient.IsSocketAvailableReading())
                            {
                                tempStr = telnetClient.Read();
                                GroundSpeed = tempStr;
                                if (Err.Equals("Timeout - GroundSpeed"))
                                {
                                    Err = "";
                                }
                            }
                            else
                            {
                                Err = "Timeout - GroundSpeed";
                            }

                            if (telnetClient.IsSocketAvailableWriting())
                            {
                                telnetClient.Write("get /instrumentation/airspeed-indicator/indicated-speed-kt");
                            }
                            else
                            {
                                Err = "Timeout - AirSpeed";
                            }
                            if (telnetClient.IsSocketAvailableReading())
                            {
                                tempStr = telnetClient.Read();
                                AirSpeed = tempStr;
                                if (Err.Equals("Timeout - AirSpeed"))
                                {
                                    Err = "";
                                }
                            }
                            else
                            {
                                Err = "Timeout - AirSpeed";
                            }

                            if (telnetClient.IsSocketAvailableWriting())
                            {
                                telnetClient.Write("get /instrumentation/gps/indicated-altitude-ft");
                            }
                            else
                            {
                                Err = "Timeout - Altitude";
                            }
                            if (telnetClient.IsSocketAvailableReading())
                            {
                                tempStr = telnetClient.Read();
                                Altitude = tempStr;
                                if (Err.Equals("Timeout - Altitude"))
                                {
                                    Err = "";
                                }
                            }
                            else
                            {
                                Err = "Timeout - Altitude";
                            }

                            if (telnetClient.IsSocketAvailableWriting())
                            {
                                telnetClient.Write("get /instrumentation/attitude-indicator/internal-roll-deg");
                            }
                            else
                            {
                                Err = "Timeout - InternalRoll";
                            }
                            if (telnetClient.IsSocketAvailableReading())
                            {
                                tempStr = telnetClient.Read();
                                InternalRoll = tempStr;
                                if (Err.Equals("Timeout - InternalRoll"))
                                {
                                    Err = "";
                                }
                            }
                            else
                            {
                                Err = "Timeout - InternalRoll";
                            }

                            if (telnetClient.IsSocketAvailableWriting())
                            {
                                telnetClient.Write("get /instrumentation/attitude-indicator/internal-pitch-deg");
                            }
                            else
                            {
                                Err = "Timeout - InternalPitch";
                            }
                            if (telnetClient.IsSocketAvailableReading())
                            {
                                tempStr = telnetClient.Read();
                                InternalPitch = tempStr;
                                if (Err.Equals("Timeout - InternalPitch"))
                                {
                                    Err = "";
                                }
                            }
                            else
                            {
                                Err = "Timeout - InternalPitch";
                            }

                            if (telnetClient.IsSocketAvailableWriting())
                            {
                                telnetClient.Write("get /instrumentation/altimeter/indicated-altitude-ft");
                            }
                            else
                            {
                                Err = "Timeout - Altimeter";
                            }
                            if (telnetClient.IsSocketAvailableReading())
                            {
                                tempStr = telnetClient.Read();
                                Altimeter = tempStr;
                                if (Err.Equals("Timeout - Altimeter"))
                                {
                                    Err = "";
                                }
                            }
                            else
                            {
                                Err = "Timeout - Altimeter";
                            }

                            if (telnetClient.IsSocketAvailableWriting())
                            {
                                telnetClient.Write("get /position/longitude-deg");
                            }
                            else
                            {
                                Err = "Timeout - Longitude";
                            }
                            if (telnetClient.IsSocketAvailableReading())
                            {
                                tempStr = telnetClient.Read();
                                if (!(tempStr.Equals("ERR")))
                                {
                                    Longitude = Double.Parse(tempStr);
                                    if (Err.Equals("Timeout - Longitude"))
                                    {
                                        Err = "";
                                    }
                                }
                            }
                            else
                            {
                                Err = "Timeout - Longitude";
                            }

                            if (telnetClient.IsSocketAvailableWriting())
                            {
                                telnetClient.Write("get /position/latitude-deg");
                            }
                            else
                            {
                                Err = "Timeout - Latitude";
                            }
                            if (telnetClient.IsSocketAvailableReading())
                            {
                                tempStr = telnetClient.Read();
                                if (!(tempStr.Equals("ERR")))
                                {
                                    Latitude = Double.Parse(tempStr);
                                    if (Err.Equals("Timeout - Latitude"))
                                    {
                                        Err = "";
                                    }
                                }
                            }
                            else
                            {
                                Err = "Timeout - Latitude";
                            }

                            Location = new Location(Latitude, Longitude);
                            mutex.ReleaseMutex();
                            Thread.Sleep(250); //SLEEP FOR 250 MS - that determines we will ask for details 4 times in a sec.
                        }
                        catch (Exception)
                        {
                            mutex.ReleaseMutex();
                            Err = "ERR";
                            Thread.Sleep(250);
                            Err = "";
                        }
                    }
                }).Start();
                new Thread(delegate ()
                {
                    while (!stop)
                    {
                        try
                        {
                            if (!errorsInIpAndPort)
                            {
                                if (telnetClient.IsSocketAvailableWriting())
                                {
                                    // send only if the queue not empty
                                    if (commandsQueue.Count != 0)
                                    {
                                        mutex.WaitOne();
                                        telnetClient.Write(commandsQueue.Dequeue());
                                        mutex.ReleaseMutex();
                                    }
                                }
                                else
                                {
                                    Err = "Timeout in set";
                                }
                            }
                            Thread.Sleep(250);
                            Err = "";
                        }
                        catch (Exception)
                        {
                            mutex.ReleaseMutex();
                            Err = "Timeout in set";
                            Thread.Sleep(250);
                            Err = "";
                        }
                    }
                }).Start();
            }
        }
        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public void SendCommandToSimulator(string command)
        {
            commandsQueue.Enqueue(command);
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

        [Obsolete]
        public string IPaddress { get { return dm_IPaddress; } set { dm_IPaddress = value; } }

        [Obsolete]
        public string Port { get { return dm_port; } set { dm_port = value; } }
    }
}
