using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShipTimesheet.API.Entities
{
    public class ShipEntity
    {
        [Key]
        public int ShipId { get; set; }
        public string Name { get; set; }
        public string IdNumber { get; set; }
        public int SkipperId { get; set; }
        public virtual SkipperEntity Skipper { get; set; }
        public virtual List<EventEntity> Events { get; set; }

    }
}
