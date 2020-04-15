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
                sender = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                // Connect the socket to the remote endpoint. Catch any errors.  
                try
                {
                    // Set the timeout for synchronous receive methods to 10 seconds (10000 milliseconds.)
                    sender.ReceiveTimeout = 10000;
                    sender.Connect(remoteEP);
                    Console.WriteLine("Socket connected to {0}", this.sender.RemoteEndPoint.ToString());
                }
                catch (ArgumentNullException ane)
                {
                    Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                    throw ane;
                }
                catch (SocketException se)
                {
                    Console.WriteLine("SocketException : {0}", se.ToString());
                    throw se;
                }
                catch (TimeoutException te)
                {
                    Console.WriteLine("TimeoutException : {0}", te.ToString());
                    throw te;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Unexpected exception : {0}", e.ToString());
                    throw e;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                throw e;
            }
        }
        public void disconnect()
        {
            try
            {
                // Release the socket.  
                sender.Shutdown(SocketShutdown.Both);
                sender.Close();
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
                int bytesRec = sender.Receive(bytes);
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
            return "ERR";
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
