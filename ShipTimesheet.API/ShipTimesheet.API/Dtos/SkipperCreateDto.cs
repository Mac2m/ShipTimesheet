using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShipTimesheet.API.Dtos
{
    public class SkipperCreateDto
    {
        [Required]
        public string Name { get; set; }
    }
}
