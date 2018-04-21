using KitchenSensor.Models;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Xml.Serialization;
using UdpBase;

namespace KitchenSensor.Services
{
    public class UDPBroadCastService : UdpBaseClass
    {

        private const string _ip = "127.0.0.1";

        private const int palinPort = 5000;
        private const int jsonPort = 5001;
        private const int xmlPort = 5002;
        
        IPEndPoint plainEp = new IPEndPoint(IPAddress.Parse(_ip), palinPort);
        IPEndPoint jsonEp = new IPEndPoint(IPAddress.Parse(_ip), jsonPort);
        IPEndPoint xmlEp = new IPEndPoint(IPAddress.Parse(_ip), xmlPort);

        Random rand = new Random(DateTime.Now.Millisecond);

        public void Start()
        {
            Socket simpleSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            simpleSocket.EnableBroadcast = true;

            Socket jsonSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            jsonSocket.EnableBroadcast = true;

            Socket xmlSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            xmlSocket.EnableBroadcast = true;

            Measurement data;

            while (true)
            {
                // Constructing an object called "data" is a type Measurement with random light&temp
                data = new Measurement(rand.Next(255), rand.Next(400));

                //Sending simple Plain data

                String buffer = String.Format($"{data}\n");

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(buffer);
                byte[] decodedBuffer = Encoding.ASCII.GetBytes(buffer);
                                
                Client.Send(decodedBuffer, decodedBuffer.Length, plainEp);


                //Sending Json data

                Console.ForegroundColor = ConsoleColor.Magenta;
                String jsonBuffer = JsonConvert.SerializeObject(data);
                Console.WriteLine(jsonBuffer);
                Console.WriteLine();

                byte[] decodedBufferJson = Encoding.ASCII.GetBytes(jsonBuffer);                
                jsonSocket.SendTo(decodedBufferJson, jsonEp);

                //// Sending XML

                Console.ForegroundColor = ConsoleColor.Green;
                String xmlBuffer = SerializeObjectToXmlString(data);
                Console.WriteLine(xmlBuffer);
                Console.WriteLine();

                byte[] decodedBufferXml = Encoding.ASCII.GetBytes(xmlBuffer);                
                xmlSocket.SendTo(decodedBufferXml, xmlEp);

                Console.Beep();

                // Put it to "sleep" before the loop goes over again:
                Thread.Sleep(30000);
            }

        }

        // Helper method for XML serialization
        public static string SerializeObjectToXmlString(Measurement toSerialize)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(toSerialize.GetType());

            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, toSerialize);
                return textWriter.ToString();
            }
        }
    }
}
