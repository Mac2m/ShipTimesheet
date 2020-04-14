using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ShipTimesheet.API.Repositories;

namespace ShipTimesheet.API.Services
{
    public class TcpClientService : BackgroundService
    {
        private readonly ILogger<TcpClientService> _logger;
        private readonly IServiceScopeFactory scopeFactory;
        private readonly IConfiguration _configuration;

        public TcpClientService(ILogger<TcpClientService> logger,
            IServiceScopeFactory scopeFactory, IConfiguration configuration)
        {
            _logger = logger;
            this.scopeFactory = scopeFactory;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            TcpClient client = new TcpClient();
            await client.Client.ConnectAsync(_configuration["TcpServerIp"], Convert.ToInt32(_configuration["TcpServerPort"]));

            _logger.LogDebug("TcpClientService is starting");
            stoppingToken.Register(() => _logger.LogDebug("TcpClientService is stopping."));
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogDebug("TcpClientService is running in background");
                using (var scope = scopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<ShipTimesheetDbContext>();
                    if (dbContext.ChangeTracker.HasChanges())
                    {
                        if (client.GetStream().CanWrite)
                        {
                            client.Client.Send(Encoding.ASCII.GetBytes("Event changed."));
                            
                        }
                    }
                    client.Close();
                    await Task.Delay(1000 * 60 * 5, stoppingToken);
                }
            }
            
            _logger.LogDebug("Demo service is stopping");
        }
    }
}
