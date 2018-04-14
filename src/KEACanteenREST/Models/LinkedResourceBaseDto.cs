using System.Collections.Generic;

namespace KEACanteenREST.Models
{
    public abstract class LinkedResourceBaseDto
    {
        public List<LinkDto> Links { get; set; } = new List<LinkDto>();
    }
}
