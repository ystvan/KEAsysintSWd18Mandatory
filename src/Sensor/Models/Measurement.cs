using System;

namespace KitchenSensor.Models
{
    [Serializable()]
    public class Measurement
    {
        [System.Xml.Serialization.XmlElement("Id")]
        public Guid Id { get; set; }

        [System.Xml.Serialization.XmlElement("Light")]
        public int Light { get; set; }

        [System.Xml.Serialization.XmlElement("Temperature")]
        public int Temperature { get; set; }

        [System.Xml.Serialization.XmlElement("Timestamp")]
        public string Timestamp { get; set; } 
        
        //parameterless default constructor to be serealised
        public Measurement() { }
      
        public Measurement(int light, int temp)
        {
            this.Light = light;
            this.Temperature = temp;
            Timestamp = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
            Id = Guid.NewGuid();
        }

        public override string ToString()
        {
            return $"Reported info from the KEA kitchen:\n\nCurrent light is: {Light} lumen\nCurrent temperature is {Temperature} Celsius\nRecorded at: {Timestamp}";
        }

    }
}
