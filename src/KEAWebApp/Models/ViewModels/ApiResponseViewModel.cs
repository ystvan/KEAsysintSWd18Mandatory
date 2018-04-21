using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KEAWebApp.Models.ViewModels
{
    public class ApiResponseViewModel
    {
        public string StatusCode { get; set; }
        public string Content { get; set; }
        public string Message { get; set; }      
    }
}
