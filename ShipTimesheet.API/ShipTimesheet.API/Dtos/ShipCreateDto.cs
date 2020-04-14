using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ShipTimesheet.API.Entities;

namespace ShipTimesheet.API.Dtos
{
    public class ShipCreateDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public int IdNumber { get; set; }
        public int SkipperId { get; set; }

    }
}
