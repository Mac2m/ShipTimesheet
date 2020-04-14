using ShipTimesheet.API.Entities;
using ShipTimesheet.API.Repositories;
using System;
using System.Threading.Tasks;

namespace ShipTimesheet.API.Services
{
    public class SeedDataService : ISeedDataService
    {
        public async Task Initialize(ShipTimesheetDbContext context)
        {
            context.Ships.Add(new ShipEntity() { Name = "Nałęczowianka", IdNumber = "22", SkipperId = 1});
            context.Ships.Add(new ShipEntity() { Name = "Borsuk", IdNumber = "12", SkipperId = 1});
            context.Ships.Add(new ShipEntity() { Name = "Ostrowianka", IdNumber = "32", SkipperId = 2});

            context.Skippers.Add(new SkipperEntity() { Name = "Skipper1"});
            context.Skippers.Add(new SkipperEntity() { Name = "Skipper2" });

            context.Events.Add(new EventEntity() { EventType = EventType.Arrival, EventTime = DateTime.Now, ShipId = 1});
            context.Events.Add(new EventEntity() { EventType = EventType.Arrival, EventTime = DateTime.Now - TimeSpan.FromDays(1), ShipId = 2 });
            context.Events.Add(new EventEntity() { EventType = EventType.Departure, EventTime = DateTime.Now - TimeSpan.FromHours(2), ShipId = 2 });

            await context.SaveChangesAsync();
        }
    }
}
