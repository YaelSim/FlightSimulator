using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace FlightSimulatorApp.Utilities
{
    public class TelnetClient : ITelnet
    {
        private Socket sender;
        private readonly Mutex mutex = new Mutex();

        // Connect to a remote device.  
        public void Connect(string ip, int port)
        {
            try
            {
                //Parse the given string ip to an IPAddress Object.
                IPAddress ipAddress = IPAddress.Parse(ip);
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, port);

                // Create a TCP/IP  socket.  
                sender = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                // Connect the socket to the remote endpoint. Catch any errors.  
                try
                {
                    // Set the timeout for synchronous receive methods to 10 seconds (10000 milliseconds.)
                    sender.ReceiveTimeout = 10000;
                    sender.Connect(remoteEP);
                    Debug.WriteLine("Socket connected to {0}", this.sender.RemoteEndPoint.ToString());
                }
                catch (ArgumentNullException ane)
                {
                    //Debug.WriteLine("ArgumentNullException : {0}", ane.ToString());
                    throw ane;
                }
                catch (SocketException se)
                {
                   // Debug.WriteLine("SocketException : {0}", se.ToString());
                    throw se;
                }
                catch (TimeoutException te)
                {
                   // Debug.WriteLine("TimeoutException : {0}", te.ToString());
                    throw te;
                }
                catch (ObjectDisposedException ode)
                {
                   // Debug.WriteLine("ODE exception : {0}", ode.ToString());
                    throw ode;
                }
                catch (Exception e)
                {
                   // Debug.WriteLine("Unexpected exception : {0}", e.ToString());
                    throw e;
                }
            }
            catch (Exception e)
            {
               // Debug.WriteLine(e.ToString());
                throw e;
            }
        }
        public void Disconnect()
        {
            try
            {
                // Release the socket.  
                sender.Shutdown(SocketShutdown.Both);
            }
            catch (ArgumentNullException)
            {
            }
            catch (SocketException)
            {
            }
            catch (ObjectDisposedException)
            {
            }
            catch (Exception)
            {
            }
            finally
            {
                sender.Close();
            }
        }
        public string Read()
        {
            try
            {
                mutex.WaitOne();
                // Data buffer for incoming data.  
                byte[] bytes = new byte[1024];

                // Receive the response from the remote device.  
                int bytesRec = sender.Receive(bytes);
                string strGotten = Encoding.ASCII.GetString(bytes, 0, bytesRec);
                Debug.WriteLine("Echoed message = {0}", strGotten);
                mutex.ReleaseMutex();
                return strGotten;
            }
            catch (ArgumentNullException)
            {
                mutex.ReleaseMutex();
                //If any exception was caught, return "0"
                return "ERR";
            }
            catch (SocketException)
            {
                mutex.ReleaseMutex();
                //If any exception was caught, return "0"
                return "ERR";
            }
            catch (ObjectDisposedException)
            {
                mutex.ReleaseMutex();
                return "ERR";
            }
            catch (Exception)
            {
                mutex.ReleaseMutex();
                //If any exception was caught, return "0"
                return "ERR";
            }
        }
        public void Write(string commandStr)
        {
            try
            {
                mutex.WaitOne();
                // Encode the data string into a byte array.  
                byte[] messageBytesArr = Encoding.ASCII.GetBytes(commandStr + "\n");

                // Send the data through the socket.  
                int bytesSent = sender.Send(messageBytesArr);
            }
            catch (ArgumentNullException ane)
            {
                throw ane;
            }
            catch (SocketException se)
            {
                throw se;
            }
            catch (ObjectDisposedException oe)
            {
                throw oe;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                mutex.ReleaseMutex();
            }
        }
        public bool IsSocketAvailableReading()
        {
            try
            {
                int microSeconds = 10000000;
                return sender.Poll(microSeconds, SelectMode.SelectRead);
            } catch (SocketException)
            {
                return false;
            }
            catch (ObjectDisposedException)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool IsSocketAvailableWriting()
        {
            try
            {
                int microSeconds = 10000000;
                return sender.Poll(microSeconds, SelectMode.SelectWrite);
            } catch (SocketException)
            {
                return false;
            }
            catch (ObjectDisposedException)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
            }

        }
    }
}
