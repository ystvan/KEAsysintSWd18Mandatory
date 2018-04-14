using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KEACanteenREST.Models
{
    public class LinkedCollectionResourceDto<T> : LinkedResourceBaseDto where T : LinkedResourceBaseDto
    {
        public IEnumerable<T> Value { get; set; }

        public LinkedCollectionResourceDto(IEnumerable<T> value)
        {
            Value = value;
        }
    }
}
