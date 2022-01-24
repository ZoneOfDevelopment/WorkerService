using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using WorkerService.Service;

namespace WorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IReadMessage _readMessage;

        public Worker(IReadMessage readMessage, ILogger<Worker> logger)
        {
            _logger = logger;
            _readMessage = readMessage;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                // Run the Read method
                await Task.Run(() => _readMessage.Read());
            }
        }
    }
}
