using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShipTimesheet.API.Entities;
using ShipTimesheet.API.Models;

namespace ShipTimesheet.API.Repositories
{
    public interface IEventRepository
    {
        EventEntity GetSingle(int id);
        void Add(EventEntity item);
        void Delete(int id);
        EventEntity Update(int id, EventEntity item);
        IQueryable<EventEntity> GetAll(QueryParameters queryParameters);

        int Count();

        bool Save();
    }
}
