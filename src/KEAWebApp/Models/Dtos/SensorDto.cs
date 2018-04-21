using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KEAWebApp.Models.Dtos
{
    public class SensorDto
    {                
        public Guid Id { get; set; }               
        public int Light { get; set; }        
        public int Temperature { get; set; }        
        public string TimeStamp { get; set; }
    }
}
