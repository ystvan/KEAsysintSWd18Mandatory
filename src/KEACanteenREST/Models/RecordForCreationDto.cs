using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KEACanteenREST.Models
{
    public class RecordForCreationDto
    {
        [Required(ErrorMessage = "Location Id cannot be empty")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Light mesurement cannot be null")]
        public int Light { get; set; }

        [Required(ErrorMessage = "Temperature measurement cannot be null")]
        public int Temperature { get; set; }

        [Required(ErrorMessage = "Time of recording cannot be empty")]
        public string Timestamp { get; set; }

    }
}
