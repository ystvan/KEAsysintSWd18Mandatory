using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KEAWebApp.Models
{
    public class Data
    {
        [DisplayName("Where?")]
        public Guid LocationIdentifier { get; set; }

        [DisplayName("Light-shining")]
        public int Light { get; set; }

        [DisplayName("Celsius grade")]
        public int Temperature { get; set; }

        [DisplayName("When?")]        
        public string RecordedAt { get; set; }
    }

    
}
