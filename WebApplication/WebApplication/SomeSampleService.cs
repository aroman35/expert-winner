using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WebApplication
{
    public class SomeSampleService : BackgroundService
    {
        private readonly ServiceIdProvider _serviceIdProvider;
        private readonly ILogger<SomeSampleService> _logger;

        public SomeSampleService(ILogger<SomeSampleService> logger, ServiceIdProvider serviceIdProvider)
        {
            _logger = logger;
            _serviceIdProvider = serviceIdProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Server '{ServerId}' is running {ThreadId}",
                    _serviceIdProvider.ServiceId.ToString("N"),
                    Thread.CurrentThread.ManagedThreadId);

                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}