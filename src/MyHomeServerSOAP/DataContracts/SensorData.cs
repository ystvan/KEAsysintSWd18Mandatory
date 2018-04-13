using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace CloudSOAP.DataContracts
{
    [DataContract]
    public class SensorData
    {
        [DataMember]
        [Key]
        [Required]
        public Guid Id { get; set; }

        [DataMember]
        [Required]
        public int Light { get; set; }

        [DataMember]
        [Required]
        public int Temperature { get; set; }

        [DataMember]
        [Required]
        public string Timestamp { get; set; }

    }
}