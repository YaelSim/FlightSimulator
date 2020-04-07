using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace FlightSimulatorApp.Utilities
{
    public interface ITelnet
    {
        void connect(string ip, int port);
        void disconnect();
        // Blocking call:
        string read();
        void write(string commandStr);
    }
}
