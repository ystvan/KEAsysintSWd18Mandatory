using HotelSecuritySensor.Models;
using Newtonsoft.Json;
using SensorDataService;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace SmartCity911.Services
{
    public static class UDPStreamReceiver
    {
        
        public static void Start()
        {

            IPEndPoint ep = new IPEndPoint(IPAddress.Any, 0);           

            UdpClient simpleSocket = new UdpClient(5000);
            UdpClient jsonSocket = new UdpClient(5001);
            UdpClient xmlSocket = new UdpClient(5002);

            while (true)
            {
                //PLAIN                
                Byte[] plainBuffer = simpleSocket.Receive(ref ep);

                String encodedText = Encoding.ASCII.GetString(plainBuffer);

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"{encodedText}\n");

                //JSON
                Byte[] jsonBuffer = jsonSocket.Receive(ref ep);
                String encodedJson = Encoding.ASCII.GetString(jsonBuffer);

                Measurement jsonMeasurement = JsonConvert.DeserializeObject<Measurement>(encodedJson);

                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"{jsonMeasurement}");
                Console.WriteLine();


                try
                {
                    SensorServiceClient azureClient = new SensorServiceClient(SensorServiceClient.EndpointConfiguration.BasicHttpBinding_ISensorService);

                    SensorData sensorData = new SensorData {
                        Id = jsonMeasurement.Id,
                        Light = jsonMeasurement.Light,
                        Temperature = jsonMeasurement.Temperature,
                        Timestamp = jsonMeasurement.Timestamp
                    };

                    azureClient.StoreDataAsync(sensorData);
                    

                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error calling Azure service {e.Message}", ConsoleColor.Cyan);
                    break;
                }

                try
                {
                    TwitterHandler.InitAuth();
                    
                    if (jsonMeasurement.Temperature <= 40)
                    {
                        var tweet = $"All good! Go back to sleep! Lights seems normal and the current temperature is {jsonMeasurement.Temperature} °C #keasysint";
                        TwitterHandler.Alarm(tweet);
                    }
                    else if (41 <= jsonMeasurement.Temperature && jsonMeasurement.Temperature <= 100)
                    {
                        var tweet = $"Something is not right, please ask for feedback and report to Captain Holt! Current temperature is {jsonMeasurement.Temperature} °C! Address Refnumber: {jsonMeasurement.Id} #keasysint";
                        TwitterHandler.Alarm(tweet);
                    }
                    else if(100 < jsonMeasurement.Temperature)
                    {
                        var tweet = $"ALARM! The roof is on fire!! The temperature is {jsonMeasurement.Temperature} °C! Go to the designated address ! Reported at {jsonMeasurement.Timestamp}, hurry up! #keasysint";
                        TwitterHandler.Alarm(tweet);
                    }

                    

                }
                catch (Exception exc)
                {
                    Console.WriteLine($"Error calling Twitter service {exc.Message}", ConsoleColor.Cyan);
                    break;
                }

                
                
   
                //XML
                
                Byte[] xmlBuffer = xmlSocket.Receive(ref ep);
                String encodedXml = Encoding.ASCII.GetString(xmlBuffer);

                var m = XmlDeserializeFromString<Measurement>(encodedXml);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"{m}");
                Console.WriteLine();


            }


        }


        //HELPER from: https://stackoverflow.com/a/2347661/8162054
        public static string XmlSerializeToString(this object objectInstance)
        {
            var serializer = new XmlSerializer(objectInstance.GetType());
            var sb = new StringBuilder();

            using (TextWriter writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, objectInstance);
            }

            return sb.ToString();
        }

        //HELPER from: https://stackoverflow.com/a/2347661/8162054
        public static T XmlDeserializeFromString<T>(this string objectData)
        {
            return (T)XmlDeserializeFromString(objectData, typeof(T));
        }

        //HELPER from: https://stackoverflow.com/a/2347661/8162054
        public static object XmlDeserializeFromString(this string objectData, Type type)
        {
            var serializer = new XmlSerializer(type);
            object result;

            using (TextReader reader = new StringReader(objectData))
            {
                result = serializer.Deserialize(reader);
            }

            return result;
        }
    }
}
