using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using KitchenSensor.Services;

namespace KitchenSensor
{
    class Program
    {

        static void Main(string[] args)
        {
            
            Console.Title = "UDP Server - Kitchen Sensor IOT widget";
            var server = new UDPBroadCastService();
            server.Start();                          

        }       
        
    }

}
