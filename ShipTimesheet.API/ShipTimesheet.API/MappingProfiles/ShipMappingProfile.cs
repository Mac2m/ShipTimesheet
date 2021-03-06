﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ShipTimesheet.API.Dtos;
using ShipTimesheet.API.Entities;

namespace ShipTimesheet.API.MappingProfiles
{
    public class ShipMappingProfile : Profile
    {
        public ShipMappingProfile()
        {
            CreateMap<ShipEntity, ShipDto>().ReverseMap();
            CreateMap<ShipEntity, ShipCreateDto>().ReverseMap();

        }
    }
}
