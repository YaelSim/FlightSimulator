using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace FlightSimulatorApp.Utilities
{
    public class TelnetClient : ITelnet
    {
        private Socket sender;
        //private int timeout = 1000;
        private Mutex mutex = new Mutex();

        // Connect to a remote device.  
        public void connect(string ip, int port)
        {
            try
            {
                //Parse the given string ip to an IPAddress Object.
                IPAddress ipAddress = IPAddress.Parse(ip);
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, port);

                // Create a TCP/IP  socket.  
                this.sender = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                // Connect the socket to the remote endpoint. Catch any errors.  
                try
                {
                    this.sender.Connect(remoteEP);
                    Console.WriteLine("Socket connected to {0}", this.sender.RemoteEndPoint.ToString());
                }
                catch (ArgumentNullException ane)
                {
                    Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                }
                catch (SocketException se)
                {
                    Console.WriteLine("SocketException : {0}", se.ToString());
                }
                catch (Exception e)
                {
                    Console.WriteLine("Unexpected exception : {0}", e.ToString());
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        public void disconnect()
        {
            try
            {
                // Release the socket.  
                this.sender.Shutdown(SocketShutdown.Both);
                this.sender.Close();
            }
            catch (ArgumentNullException ane)
            {
                Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
            }
            catch (SocketException se)
            {
                Console.WriteLine("SocketException : {0}", se.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected exception : {0}", e.ToString());
            }
        }
        public string read()
        {
            try
            {
                mutex.WaitOne();
                // Data buffer for incoming data.  
                byte[] bytes = new byte[1024];

                // Receive the response from the remote device.  
                int bytesRec = this.sender.Receive(bytes);
                string strGotten = Encoding.ASCII.GetString(bytes, 0, bytesRec);
                Console.WriteLine("Echoed message = {0}", strGotten);
                return strGotten;
            }
            catch (ArgumentNullException ane)
            {
                Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
            }
            catch (SocketException se)
            {
                Console.WriteLine("SocketException : {0}", se.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected exception : {0}", e.ToString());
            }
            finally
            {
                mutex.ReleaseMutex();
            }
            //If any exception was caught, return "0"
            return "0";
        }
        public void write(string commandStr)
        {
            try
            {
                mutex.WaitOne();
                // Encode the data string into a byte array.  
                //byte[] messageBytesArr = Encoding.ASCII.GetBytes(commandStr + "<EOF>");
                byte[] messageBytesArr = Encoding.ASCII.GetBytes(commandStr + "\n");

                // Send the data through the socket.  
                int bytesSent = this.sender.Send(messageBytesArr);
            }
            catch (ArgumentNullException ane)
            {
                Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
            }
            catch (SocketException se)
            {
                Console.WriteLine("SocketException : {0}", se.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected exception : {0}", e.ToString());
            }
            finally
            {
                mutex.ReleaseMutex();
            }
        }
    }
}
