using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KEACanteenREST.Models
{
    public class RecordDto
    {
        public Guid LocationIdentifier { get; set; }
        public int Light { get; set; }
        public int Temperature { get; set; }
        public string RecordedAt { get; set; }
    }
}
