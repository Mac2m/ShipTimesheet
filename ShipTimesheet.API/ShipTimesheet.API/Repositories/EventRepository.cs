using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShipTimesheet.API.Entities;
using ShipTimesheet.API.Helpers;
using ShipTimesheet.API.Models;
using ShipTimesheet.API.Services;

namespace ShipTimesheet.API.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly ShipTimesheetDbContext _dbContext;

        public EventRepository(ShipTimesheetDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public EventEntity GetSingle(int id)
        {
            return _dbContext.Events.Include(x => x.Ship).ThenInclude(y => y.Skipper).FirstOrDefault(x => x.EventId == id);
        }

        public void Add(EventEntity item)
        {
            _dbContext.Events.Add(item);
        }

        public void Delete(int id)
        {
            EventEntity item = GetSingle(id);
            _dbContext.Events.Remove(item);
        }

        public EventEntity Update(int id, EventEntity item)
        {
            _dbContext.Events.Update(item);
            return item;
        }

        public IQueryable<EventEntity> GetAll(QueryParameters queryParameters)
        {
            IQueryable<EventEntity> _allItems = _dbContext.Events.Include(x => x.Ship).ThenInclude(y => y.Skipper).OrderByDescending(x => x.EventTime);

            if (queryParameters.HasQuery())
            {
                _allItems = _allItems
                    .Where(x => x.EventTime.ToString(CultureInfo.CurrentCulture).Contains(queryParameters.Query.ToLowerInvariant())
                    || x.EventType.ToString().Contains(queryParameters.Query, StringComparison.InvariantCultureIgnoreCase));
            }

            return _allItems
                .Skip(queryParameters.PageCount * (queryParameters.Page - 1))
                .Take(queryParameters.PageCount);
        }

        public int Count()
        {
            return _dbContext.Events.Count();
        }

        public bool Save()
        {
            return (_dbContext.SaveChanges() >= 0);
        }

    }
}
