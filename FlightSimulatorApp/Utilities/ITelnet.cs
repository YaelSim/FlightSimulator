using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace FlightSimulatorApp.Utilities
{
    public interface ITelnet
    {
        void Connect(string ip, int port);
        void Disconnect();
        // Blocking call:
        string Read();
        void Write(string commandStr);
        bool IsSocketAvailableWriting();
        bool IsSocketAvailableReading();

    }
}
