using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShipTimesheet.API.Entities
{
    public class SkipperEntity
    {
        [Key]
        public int SkipperId { get; set; }
        public string Name { get; set; }
        public virtual List<ShipEntity> Ships { get; set; }
    }
}