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
    public class SkippersController : ControllerBase
    {
        private readonly ISkipperRepository _skipperRepository;
        private readonly IUrlHelper _urlHelper;
        private readonly IMapper _mapper;

        public SkippersController(
            IUrlHelper urlHelper,
            ISkipperRepository skipperRepository,
            IMapper mapper)
        {
            _skipperRepository = skipperRepository;
            _mapper = mapper;
            _urlHelper = urlHelper;
        }

        [HttpGet(Name = nameof(GetAllSkippers))]
        public ActionResult GetAllSkippers(ApiVersion version, [FromQuery] QueryParameters queryParameters)
        {
            List<SkipperEntity> skipperItems = _skipperRepository.GetAll(queryParameters).ToList();

            var allItemCount = _skipperRepository.Count();

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

            var toReturn = skipperItems.Select(x => ExpandSingleSkipperItem(x, version));

            return Ok(new
            {
                value = toReturn,
                links = links
            });
        }

        [HttpGet]
        [Route("{id:int}", Name = nameof(GetSingleSkipper))]
        public ActionResult GetSingleSkipper(ApiVersion version, int id)
        {
            SkipperEntity skipperItem = _skipperRepository.GetSingle(id);

            if (skipperItem == null)
            {
                return NotFound();
            }

            return Ok(ExpandSingleSkipperItem(skipperItem, version));
        }

        [HttpPost(Name = nameof(AddSkipper))]
        public ActionResult<SkipperDto> AddSkipper(ApiVersion version, [FromBody] SkipperCreateDto skipperCreateDto)
        {
            if (skipperCreateDto == null)
            {
                return BadRequest();
            }

            SkipperEntity toAdd = _mapper.Map<SkipperEntity>(skipperCreateDto);

            _skipperRepository.Add(toAdd);

            if (!_skipperRepository.Save())
            {
                throw new Exception("Creating a skipper failed on save.");
            }

            SkipperEntity newSkipperItem = _skipperRepository.GetSingle(toAdd.SkipperId);

            return CreatedAtRoute(nameof(GetSingleSkipper), new { version = version.ToString(), id = newSkipperItem.SkipperId },
                _mapper.Map<SkipperDto>(newSkipperItem));
        }

        private List<LinkDto> CreateLinksForCollection(QueryParameters queryParameters, int totalCount, ApiVersion version)
        {
            var links = new List<LinkDto>();

            // self 
            links.Add(new LinkDto(_urlHelper.Link(nameof(GetAllSkippers), new
            {
                pagecount = queryParameters.PageCount,
                page = queryParameters.Page,
                orderby = queryParameters.OrderBy
            }), "self", "GET"));

            links.Add(new LinkDto(_urlHelper.Link(nameof(GetAllSkippers), new
            {
                pagecount = queryParameters.PageCount,
                page = 1,
                orderby = queryParameters.OrderBy
            }), "first", "GET"));

            links.Add(new LinkDto(_urlHelper.Link(nameof(GetAllSkippers), new
            {
                pagecount = queryParameters.PageCount,
                page = queryParameters.GetTotalPages(totalCount),
                orderby = queryParameters.OrderBy
            }), "last", "GET"));

            if (queryParameters.HasNext(totalCount))
            {
                links.Add(new LinkDto(_urlHelper.Link(nameof(GetAllSkippers), new
                {
                    pagecount = queryParameters.PageCount,
                    page = queryParameters.Page + 1,
                    orderby = queryParameters.OrderBy
                }), "next", "GET"));
            }

            if (queryParameters.HasPrevious())
            {
                links.Add(new LinkDto(_urlHelper.Link(nameof(GetAllSkippers), new
                {
                    pagecount = queryParameters.PageCount,
                    page = queryParameters.Page - 1,
                    orderby = queryParameters.OrderBy
                }), "previous", "GET"));
            }

            var posturl = _urlHelper.Link(nameof(AddSkipper), new { version = version.ToString() });

            links.Add(
               new LinkDto(posturl,
               "create_skipper",
               "POST"));

            return links;
        }

        private dynamic ExpandSingleSkipperItem(SkipperEntity skipperItem, ApiVersion version)
        {
            var links = GetLinks(skipperItem.SkipperId, version);
            SkipperDto item = _mapper.Map<SkipperDto>(skipperItem);

            var resourceToReturn = item.ToDynamic() as IDictionary<string, object>;
            resourceToReturn.Add("links", links);

            return resourceToReturn;
        }

        private IEnumerable<LinkDto> GetLinks(int id, ApiVersion version)
        {
            var links = new List<LinkDto>();

            var getLink = _urlHelper.Link(nameof(GetSingleSkipper), new { version = version.ToString(), id = id });

            links.Add(
              new LinkDto(getLink, "self", "GET"));

            var createLink = _urlHelper.Link(nameof(AddSkipper), new { version = version.ToString() });

            links.Add(
              new LinkDto(createLink,
              "create_skipper",
              "POST"));

            return links;
        }
    }
}
