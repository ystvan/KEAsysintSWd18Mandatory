using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KEAWebApp.Models.ViewModels
{
    public class CreateDataViewModel
    {
        [DisplayName("Where did it occure? Remember valid Guid!")]
        [Required(ErrorMessage = "Location cannot be null")]
        public Guid LocationIdentifier { get; set; }

        [DisplayName("How light or dark was it?")]
        [Required(ErrorMessage = "Light mesurement cannot be null")]
        public int Light { get; set; }

        [DisplayName("How hot or cold did you measure")]
        [Required(ErrorMessage = "Temperature measurement cannot be null")]
        public int Temperature { get; set; }
        
    }
}
