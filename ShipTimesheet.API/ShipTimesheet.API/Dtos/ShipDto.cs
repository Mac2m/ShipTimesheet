using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShipTimesheet.API.Entities;

namespace ShipTimesheet.API.Dtos
{
    public class ShipDto
    {
        public int ShipId { get; set; }
        public string Name { get; set; }
        public string IdNumber { get; set; }
        public int SkipperId { get; set; }
        public SkipperDto Skipper { get; set; }
        public virtual List<EventDto> Events { get; set; }

    }
}
