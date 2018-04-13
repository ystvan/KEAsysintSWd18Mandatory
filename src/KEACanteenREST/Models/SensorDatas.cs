using System;
using System.Collections.Generic;

namespace KEACanteenREST.Models
{
    public partial class SensorDatas
    {
        public Guid Id { get; set; }
        public int Light { get; set; }
        public int Temperature { get; set; }
        public string Timestamp { get; set; }
    }
}
