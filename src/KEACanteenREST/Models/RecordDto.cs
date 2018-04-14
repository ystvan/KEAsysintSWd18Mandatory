using System;
using System.ComponentModel.DataAnnotations;

namespace KEACanteenREST.Models
{
    public class RecordDto : LinkedResourceBaseDto
    {
        [Required(ErrorMessage = "Location Id cannot be empty")]
        public Guid LocationIdentifier { get; set; }

        [Required(ErrorMessage = "Light mesurement cannot be null")]
        public int Light { get; set; }

        [Required(ErrorMessage = "Temperature measurement cannot be null")]
        public int Temperature { get; set; }

        [Required(ErrorMessage = "Time of recording cannot be empty")]
        public string RecordedAt { get; set; }
    }
}
