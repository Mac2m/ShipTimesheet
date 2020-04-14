using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShipTimesheet.API.Entities;

namespace ShipTimesheet.API.Dtos
{
    public class SkipperDto
    {
        public int SkipperId { get; set; }
        public string Name { get; set; }
        public virtual List<ShipDto> Ships { get; set; }
    }
}
