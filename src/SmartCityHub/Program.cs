using SmartCity911.Services;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SmartCity911
{
    class Program
    {
               
        static void Main(string[] args)
        {
            Console.Title = "Smart City Emergency Service";
            UDPStreamReceiver.Start();
                      
        }
        
    }
}
