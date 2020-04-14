using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShipTimesheet.API.Entities;

namespace ShipTimesheet.API.Dtos
{
    public class EventCreateDto
    {
        public int ShipId { get; set; }
        public EventType EventType { get; set; }
        public DateTime EventTime { get; set; }
    }
}
