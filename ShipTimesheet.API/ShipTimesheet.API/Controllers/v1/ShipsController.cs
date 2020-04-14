using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ShipTimesheet.API.Dtos;
using ShipTimesheet.API.Entities;
using ShipTimesheet.API.Helpers;
using ShipTimesheet.API.Models;
using ShipTimesheet.API.Repositories;

namespace ShipTimesheet.API.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ShipsController : ControllerBase
    {
        private readonly IShipRepository _shipRepository;
        private readonly IUrlHelper _urlHelper;
        private readonly IMapper _mapper;

        public ShipsController(
            IUrlHelper urlHelper,
            IShipRepository shipRepository,
            IMapper mapper)
        {
            _shipRepository = shipRepository;
            _mapper = mapper;
            _urlHelper = urlHelper;
        }

        [HttpGet(Name = nameof(GetAllShips))]
        public ActionResult GetAllShips(ApiVersion version, [FromQuery] QueryParameters queryParameters)
        {
            List<ShipEntity> shipItems = _shipRepository.GetAll(queryParameters).ToList();

            var allItemCount = _shipRepository.Count();

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

            var toReturn = shipItems.Select(x => ExpandSingleShipItem(x, version));

            return Ok(new
            {
                value = toReturn,
                links = links
            });
        }

        [HttpGet]
        [Route("{id:int}", Name = nameof(GetSingleShip))]
        public ActionResult GetSingleShip(ApiVersion version, int id)
        {
            ShipEntity shipItem = _shipRepository.GetSingle(id);

            if (shipItem == null)
            {
                return NotFound();
            }

            return Ok(ExpandSingleShipItem(shipItem, version));
        }

        [HttpPost(Name = nameof(AddShip))]
        public ActionResult<ShipDto> AddShip(ApiVersion version, [FromBody] ShipCreateDto shipCreateDto)
        {
            if (shipCreateDto == null)
            {
                return BadRequest();
            }

            ShipEntity toAdd = _mapper.Map<ShipEntity>(shipCreateDto);

            _shipRepository.Add(toAdd);

            if (!_shipRepository.Save())
            {
                throw new Exception("Creating a ship failed on save.");
            }

            ShipEntity newShipItem = _shipRepository.GetSingle(toAdd.ShipId);

            return CreatedAtRoute(nameof(GetSingleShip), new { version = version.ToString(), id = newShipItem.ShipId },
                _mapper.Map<ShipDto>(newShipItem));
        }

        private List<LinkDto> CreateLinksForCollection(QueryParameters queryParameters, int totalCount, ApiVersion version)
        {
            var links = new List<LinkDto>();

            // self 
            links.Add(new LinkDto(_urlHelper.Link(nameof(GetAllShips), new
            {
                pagecount = queryParameters.PageCount,
                page = queryParameters.Page,
                orderby = queryParameters.OrderBy
            }), "self", "GET"));

            links.Add(new LinkDto(_urlHelper.Link(nameof(GetAllShips), new
            {
                pagecount = queryParameters.PageCount,
                page = 1,
                orderby = queryParameters.OrderBy
            }), "first", "GET"));

            links.Add(new LinkDto(_urlHelper.Link(nameof(GetAllShips), new
            {
                pagecount = queryParameters.PageCount,
                page = queryParameters.GetTotalPages(totalCount),
                orderby = queryParameters.OrderBy
            }), "last", "GET"));

            if (queryParameters.HasNext(totalCount))
            {
                links.Add(new LinkDto(_urlHelper.Link(nameof(GetAllShips), new
                {
                    pagecount = queryParameters.PageCount,
                    page = queryParameters.Page + 1,
                    orderby = queryParameters.OrderBy
                }), "next", "GET"));
            }

            if (queryParameters.HasPrevious())
            {
                links.Add(new LinkDto(_urlHelper.Link(nameof(GetAllShips), new
                {
                    pagecount = queryParameters.PageCount,
                    page = queryParameters.Page - 1,
                    orderby = queryParameters.OrderBy
                }), "previous", "GET"));
            }

            var posturl = _urlHelper.Link(nameof(AddShip), new { version = version.ToString() });

            links.Add(
               new LinkDto(posturl,
               "create_ship",
               "POST"));

            return links;
        }

        private dynamic ExpandSingleShipItem(ShipEntity shipItem, ApiVersion version)
        {
            var links = GetLinks(shipItem.ShipId, version);
            ShipDto item = _mapper.Map<ShipDto>(shipItem);

            var resourceToReturn = item.ToDynamic() as IDictionary<string, object>;
            resourceToReturn.Add("links", links);

            return resourceToReturn;
        }

        private IEnumerable<LinkDto> GetLinks(int id, ApiVersion version)
        {
            var links = new List<LinkDto>();

            var getLink = _urlHelper.Link(nameof(GetSingleShip), new { version = version.ToString(), id = id });

            links.Add(
              new LinkDto(getLink, "self", "GET"));

            var createLink = _urlHelper.Link(nameof(AddShip), new { version = version.ToString() });

            links.Add(
              new LinkDto(createLink,
              "create_ship",
              "POST"));

            return links;
        }
    }
}
