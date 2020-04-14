using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShipTimesheet.API.Entities
{
    public class EventEntity
    {
        [Key]
        public int EventId { get; set; }
        public int ShipId { get; set; }
        public virtual ShipEntity Ship { get; set; }
        public EventType EventType { get; set; }
        public DateTime EventTime { get; set; }
    }
}
