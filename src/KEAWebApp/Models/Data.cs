using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace KEAWebApp.Models
{
    public class Data
    {
        [DisplayName("Where?")]
        public Guid Location { get; set; }

        [DisplayName("Light-shining")]
        public int Light { get; set; }

        [DisplayName("Celsius grade")]
        public int Temperature { get; set; }

        [DisplayName("When?")]
        public string Recorded { get; set; }
    }
}
