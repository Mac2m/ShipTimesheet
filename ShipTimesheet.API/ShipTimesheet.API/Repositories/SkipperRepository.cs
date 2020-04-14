using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShipTimesheet.API.Entities;
using ShipTimesheet.API.Helpers;
using ShipTimesheet.API.Models;

namespace ShipTimesheet.API.Repositories
{
    public class SkipperRepository : ISkipperRepository
    {
        private readonly ShipTimesheetDbContext _dbContext;

        public SkipperRepository(ShipTimesheetDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public SkipperEntity GetSingle(int id)
        {
            return _dbContext.Skippers.Include(x => x.Ships).FirstOrDefault(x => x.SkipperId == id);
        }

        public void Add(SkipperEntity item)
        {
            _dbContext.Skippers.Add(item);
        }

        public void Delete(int id)
        {
            SkipperEntity item = GetSingle(id);
            _dbContext.Skippers.Remove(item);
        }

        public SkipperEntity Update(int id, SkipperEntity item)
        {
            _dbContext.Skippers.Update(item);
            return item;
        }

        public IQueryable<SkipperEntity> GetAll(QueryParameters queryParameters)
        {
            IQueryable<SkipperEntity> _allItems = _dbContext.Skippers.Include(x => x.Ships).OrderByDescending(x => x.Name);

            if (queryParameters.HasQuery())
            {
                _allItems = _allItems
                    .Where(x => x.Name.Contains(queryParameters.Query, StringComparison.InvariantCultureIgnoreCase));
            }

            return _allItems
                .Skip(queryParameters.PageCount * (queryParameters.Page - 1))
                .Take(queryParameters.PageCount);
        }

        public int Count()
        {
            return _dbContext.Skippers.Count();
        }

        public bool Save()
        {
            return (_dbContext.SaveChanges() >= 0);
        }
    }
}
