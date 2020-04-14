using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShipTimesheet.API.Entities;
using ShipTimesheet.API.Models;

namespace ShipTimesheet.API.Repositories
{
    public interface IShipRepository
    {
        ShipEntity GetSingle(int id);
        void Add(ShipEntity item);
        void Delete(int id);
        ShipEntity Update(int id, ShipEntity item);
        IQueryable<ShipEntity> GetAll(QueryParameters queryParameters);

        int Count();

        bool Save();
    }
}
