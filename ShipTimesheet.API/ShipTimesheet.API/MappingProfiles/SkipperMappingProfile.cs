using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ShipTimesheet.API.Dtos;
using ShipTimesheet.API.Entities;

namespace ShipTimesheet.API.MappingProfiles
{
    public class SkipperMappingProfile : Profile   
    {
        public SkipperMappingProfile()
        {
            CreateMap<SkipperEntity, SkipperDto>().ReverseMap();
            CreateMap<SkipperEntity, SkipperCreateDto>().ReverseMap();

        }
    }
}
