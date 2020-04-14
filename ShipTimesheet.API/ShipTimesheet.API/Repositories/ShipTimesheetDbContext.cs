using Microsoft.EntityFrameworkCore;
using ShipTimesheet.API.Entities;

namespace ShipTimesheet.API.Repositories
{
    public class ShipTimesheetDbContext : DbContext
    {
        public ShipTimesheetDbContext(DbContextOptions<ShipTimesheetDbContext> options)
           : base(options)
        {

        }

        public DbSet<ShipEntity> Ships { get; set; }
        public DbSet<SkipperEntity> Skippers { get; set; }
        public DbSet<EventEntity> Events { get; set; }
    }
}
