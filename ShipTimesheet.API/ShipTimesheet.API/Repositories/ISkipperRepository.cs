using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShipTimesheet.API.Entities;
using ShipTimesheet.API.Models;

namespace ShipTimesheet.API.Repositories
{
    public interface ISkipperRepository
    {
        SkipperEntity GetSingle(int id);
        void Add(SkipperEntity item);
        void Delete(int id);
        SkipperEntity Update(int id, SkipperEntity item);
        IQueryable<SkipperEntity> GetAll(QueryParameters queryParameters);

        int Count();

        bool Save();
    }
}
