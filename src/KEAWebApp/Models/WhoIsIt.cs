using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KEAWebApp.Models
{
    public class WhoIsIt
    {
        public string Name { get; set; }
        public string  Email { get; set; }

        public override string ToString()
        {
            return $"As far as I see, you must be {Name} and your contact info is probably {Email}, am I right?";
        }

    }
}
