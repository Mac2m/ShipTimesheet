using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ShipTimesheet.API.Dtos;
using ShipTimesheet.API.Entities;

namespace ShipTimesheet.API.MappingProfiles
{
    public class EventMappingProfile : Profile   
    {
        public EventMappingProfile()
        {
            CreateMap<EventEntity, EventDto>().ReverseMap();
            CreateMap<EventEntity, EventCreateDto>().ReverseMap();
            CreateMap<EventEntity, EventUpdateDto>().ReverseMap();

        }
    }
}
