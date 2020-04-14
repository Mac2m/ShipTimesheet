using System;
using System.Linq;
using AutoMapper;
using ShipTimesheet.API.Dtos;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using ShipTimesheet.API.Repositories;
using System.Collections.Generic;
using ShipTimesheet.API.Entities;
using ShipTimesheet.API.Models;
using ShipTimesheet.API.Helpers;

namespace ShipTimesheet.API.v1.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    //[Route("api/[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly IEventRepository _eventRepository;
        private readonly IUrlHelper _urlHelper;
        private readonly IMapper _mapper;

        public EventsController(
            IUrlHelper urlHelper,
            IEventRepository eventRepository,
            IMapper mapper)
        {
            _eventRepository = eventRepository;
            _mapper = mapper;
            _urlHelper = urlHelper;
        }

        [HttpGet(Name = nameof(GetAllEvents))]
        public ActionResult GetAllEvents(ApiVersion version, [FromQuery] QueryParameters queryParameters)
        {
            List<EventEntity> eventItems = _eventRepository.GetAll(queryParameters).ToList();

            var allItemCount = _eventRepository.Count();

            var paginationMetadata = new
            {
                totalCount = allItemCount,
                pageSize = queryParameters.PageCount,
                currentPage = queryParameters.Page,
                totalPages = queryParameters.GetTotalPages(allItemCount)
            };

            Response.Headers.Add("X-Pagination",
                Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

            var links = CreateLinksForCollection(queryParameters, allItemCount, version);

            var toReturn = eventItems.Select(x => ExpandSingleEventItem(x, version));

            return Ok(new
            {
                value = toReturn,
                links = links
            });
        }

        [HttpGet]
        [Route("{id:int}", Name = nameof(GetSingleEvent))]
        public ActionResult GetSingleEvent(ApiVersion version, int id)
        {
            EventEntity eventItem = _eventRepository.GetSingle(id);

            if (eventItem == null)
            {
                return NotFound();
            }

            return Ok(ExpandSingleEventItem(eventItem, version));
        }

        [HttpPost(Name = nameof(AddEvent))]
        public ActionResult<EventDto> AddEvent(ApiVersion version, [FromBody] EventCreateDto eventCreateDto)
        {
            if (eventCreateDto == null)
            {
                return BadRequest();
            }

            EventEntity toAdd = _mapper.Map<EventEntity>(eventCreateDto);

            _eventRepository.Add(toAdd);

            if (!_eventRepository.Save())
            {
                throw new Exception("Creating an event failed on save.");
            }

            EventEntity newEventItem = _eventRepository.GetSingle(toAdd.EventId);

            return CreatedAtRoute(nameof(GetSingleEvent), new { version = version.ToString(), id = newEventItem.EventId },
                _mapper.Map<EventDto>(newEventItem));
        }

        [HttpPatch("{id:int}", Name = nameof(PartiallyUpdateEvent))]
        public ActionResult<EventDto> PartiallyUpdateEvent(int id, [FromBody] JsonPatchDocument<EventUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            EventEntity existingEntity = _eventRepository.GetSingle(id);

            if (existingEntity == null)
            {
                return NotFound();
            }

            EventUpdateDto eventUpdateDto = _mapper.Map<EventUpdateDto>(existingEntity);
            patchDoc.ApplyTo(eventUpdateDto);

            TryValidateModel(eventUpdateDto);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _mapper.Map(eventUpdateDto, existingEntity);
            EventEntity updated = _eventRepository.Update(id, existingEntity);

            if (!_eventRepository.Save())
            {
                throw new Exception("Updating an event failed on save.");
            }

            return Ok(_mapper.Map<EventDto>(updated));
        }

        [HttpDelete]
        [Route("{id:int}", Name = nameof(RemoveEvent))]
        public ActionResult RemoveEvent(int id)
        {
            EventEntity eventItem = _eventRepository.GetSingle(id);

            if (eventItem == null)
            {
                return NotFound();
            }

            
            _eventRepository.Delete(id);

            if (!_eventRepository.Save())
            {
                throw new Exception("Deleting an event failed on save.");
            }

            return Ok();
        }

        [HttpPut]
        [Route("{id:int}", Name = nameof(UpdateEvent))]
        public ActionResult<EventDto> UpdateEvent(int id, [FromBody]EventUpdateDto eventUpdateDto)
        {
            if (eventUpdateDto == null)
            {
                return BadRequest();
            }

            var existingEventItem = _eventRepository.GetSingle(id);

            if (existingEventItem == null)
            {
                return NotFound();
            }

            _mapper.Map(eventUpdateDto, existingEventItem);

            _eventRepository.Update(id, existingEventItem);

            if (!_eventRepository.Save())
            {
                throw new Exception("Updating an event failed on save.");
            }

            return Ok(_mapper.Map<EventDto>(existingEventItem));
        }

        private List<LinkDto> CreateLinksForCollection(QueryParameters queryParameters, int totalCount, ApiVersion version)
        {
            var links = new List<LinkDto>();

            // self 
            links.Add(new LinkDto(_urlHelper.Link(nameof(GetAllEvents), new
            {
                pagecount = queryParameters.PageCount,
                page = queryParameters.Page,
                orderby = queryParameters.OrderBy
            }), "self", "GET"));

            links.Add(new LinkDto(_urlHelper.Link(nameof(GetAllEvents), new
            {
                pagecount = queryParameters.PageCount,
                page = 1,
                orderby = queryParameters.OrderBy
            }), "first", "GET"));

            links.Add(new LinkDto(_urlHelper.Link(nameof(GetAllEvents), new
            {
                pagecount = queryParameters.PageCount,
                page = queryParameters.GetTotalPages(totalCount),
                orderby = queryParameters.OrderBy
            }), "last", "GET"));

            if (queryParameters.HasNext(totalCount))
            {
                links.Add(new LinkDto(_urlHelper.Link(nameof(GetAllEvents), new
                {
                    pagecount = queryParameters.PageCount,
                    page = queryParameters.Page + 1,
                    orderby = queryParameters.OrderBy
                }), "next", "GET"));
            }

            if (queryParameters.HasPrevious())
            {
                links.Add(new LinkDto(_urlHelper.Link(nameof(GetAllEvents), new
                {
                    pagecount = queryParameters.PageCount,
                    page = queryParameters.Page - 1,
                    orderby = queryParameters.OrderBy
                }), "previous", "GET"));
            }

            var posturl = _urlHelper.Link(nameof(AddEvent), new { version = version.ToString() });

            links.Add(
               new LinkDto(posturl,
               "create_event",
               "POST"));

            return links;
        }

        private dynamic ExpandSingleEventItem(EventEntity eventItem, ApiVersion version)
        {
            var links = GetLinks(eventItem.EventId, version);
            EventDto item = _mapper.Map<EventDto>(eventItem);

            var resourceToReturn = item.ToDynamic() as IDictionary<string, object>;
            resourceToReturn.Add("links", links);

            return resourceToReturn;
        }

        private IEnumerable<LinkDto> GetLinks(int id, ApiVersion version)
        {
            var links = new List<LinkDto>();

            var getLink = _urlHelper.Link(nameof(GetSingleEvent), new { version = version.ToString(), id = id });

            links.Add(
              new LinkDto(getLink, "self", "GET"));

            var deleteLink = _urlHelper.Link(nameof(RemoveEvent), new { version = version.ToString(), id = id });

            links.Add(
              new LinkDto(deleteLink,
              "delete_event",
              "DELETE"));

            var createLink = _urlHelper.Link(nameof(AddEvent), new { version = version.ToString() });

            links.Add(
              new LinkDto(createLink,
              "create_event",
              "POST"));

            var updateLink = _urlHelper.Link(nameof(UpdateEvent), new { version = version.ToString(), id = id });

            links.Add(
               new LinkDto(updateLink,
               "update_event",
               "PUT"));

            return links;
        }
    }
}
