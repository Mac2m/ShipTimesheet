using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShipTimesheet.API.Entities;
using ShipTimesheet.API.Helpers;
using ShipTimesheet.API.Models;

namespace ShipTimesheet.API.Repositories
{
    public class ShipRepository : IShipRepository
    {
        private readonly ShipTimesheetDbContext _dbContext;

        public ShipRepository(ShipTimesheetDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ShipEntity GetSingle(int id)
        {
            return _dbContext.Ships.Include(x => x.Events).Include(x => x.Skipper).FirstOrDefault(x => x.ShipId == id);
        }

        public void Add(ShipEntity item)
        {
            _dbContext.Ships.Add(item);
        }

        public void Delete(int id)
        {
            ShipEntity item = GetSingle(id);
            _dbContext.Ships.Remove(item);
        }

        public ShipEntity Update(int id, ShipEntity item)
        {
            _dbContext.Ships.Update(item);
            return item;
        }

        public IQueryable<ShipEntity> GetAll(QueryParameters queryParameters)
        {
            IQueryable<ShipEntity> _allItems = _dbContext.Ships.Include(x => x.Events).Include(x => x.Skipper).OrderByDescending(x => x.Name);

            if (queryParameters.HasQuery())
            {
                _allItems = _allItems
                    .Where(x => x.IdNumber.ToString().Contains(queryParameters.Query.ToLowerInvariant())
                                || x.Name.Contains(queryParameters.Query, StringComparison.InvariantCultureIgnoreCase));
            }

            return _allItems
                .Skip(queryParameters.PageCount * (queryParameters.Page - 1))
                .Take(queryParameters.PageCount);
        }

        public int Count()
        {
            return _dbContext.Ships.Count();
        }

        public bool Save()
        {
            return (_dbContext.SaveChanges() >= 0);
        }
    }
}
