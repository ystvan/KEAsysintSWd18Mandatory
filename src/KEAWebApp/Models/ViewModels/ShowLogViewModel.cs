using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KEAWebApp.Models.ViewModels
{
    public class ShowLogViewModel
    {
        public IEnumerable<Log> Logs { get; } = new List<Log>();

        public ShowLogViewModel(List<Log> logs)
        {
            Logs = logs;
        }
    }
}
