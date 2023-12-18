using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Datagram_Protocol1
{
    class Program
    {
        static UdpClient udpServer;
        static UdpClient udpClient;

        static void Main()
        {
            StartServer();
            StartClient();
        }

        static void StartServer()
        {
            udpServer = new UdpClient(8080);
            Thread serverThread = new Thread(new ThreadStart(SendTime));
            serverThread.Start();
        }

        static void SendTime()
        {
            while (true)
            {
                UdpClient udpServer = new UdpClient(8080);
                while (true)
                {
                    byte[] timeBytes = Encoding.UTF8.GetBytes(DateTime.Now.ToLongTimeString());
                    udpServer.Send(timeBytes, timeBytes.Length, "192.168.0.100", 8081);
                    System.Threading.Thread.Sleep(1000);
                }
            }
        }

        static void StartClient()
        {
            udpClient = new UdpClient(8081);
            Thread clientThread = new Thread(new ThreadStart(GetTime));
            clientThread.Start();
        }

        static void GetTime()
        {
            while (true)
            {
                UdpClient udpClient = new UdpClient(8081);
                IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
                while (true)
                {
                    byte[] timeBytes = udpClient.Receive(ref remoteEndPoint);
                    string time = Encoding.UTF8.GetString(timeBytes);
                    Console.WriteLine(time);
                }
            }
        }
    }
}
