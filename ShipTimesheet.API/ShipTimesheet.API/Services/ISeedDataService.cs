using ShipTimesheet.API.Repositories;
using System.Threading.Tasks;

namespace ShipTimesheet.API.Services
{
    public interface ISeedDataService
    {
        Task Initialize(ShipTimesheetDbContext context);
    }
}
