using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KEAWebApp.Models.ViewModels
{
    public class WhoIsItViewModel
    {
        public WhoIsIt WhoIsIt { get; set; }

        public WhoIsItViewModel(WhoIsIt whoIsIt)
        {
            WhoIsIt = whoIsIt;
        }

    }
}
