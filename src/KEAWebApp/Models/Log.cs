using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace KEAWebApp.Models
{
    public class Log
    {
        [DisplayName("ID Number")]
        public int Id { get; set; }

        [DisplayName("Message")]
        public string Message { get; set; }

        [DisplayName("Such as")]
        public string MessageTemplate { get; set; }

        [DisplayName("Severity")]
        public string Level { get; set; }

        [DisplayName("When?")]
        public DateTime TimeStamp { get; set; }

        [DisplayName("Exception Thrown")]
        public string Exception { get; set; }

        [DisplayName("N/A")]
        public string Properties { get; set; }

    }
}
