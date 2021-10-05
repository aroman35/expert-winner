using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WebApplication
{
    public class HostsManager : IDisposable
    {
        private readonly ILogger<HostsManager> _logger;
        private readonly ConcurrentDictionary<string, IHost> _runningHosts;

        public HostsManager(ILogger<HostsManager> logger)
        {
            _logger = logger;
            _runningHosts = new ConcurrentDictionary<string, IHost>();
        }

        public Guid AddHost()
        {
            var appSettings = File.OpenRead("sample-appsettings.json");
            var hostId = Guid.NewGuid();
            var host = HostFactory.CreateHost(appSettings, Environments.Production, hostId);

            _runningHosts.TryAdd(hostId.ToString("N"), host);
            Task.Factory.StartNew(() => host.StartAsync());
            
            _logger.LogInformation("A new host '{HostId}' had been added", hostId);

            return hostId;
        }

        public void Dispose()
        {
            foreach (var (_, host) in _runningHosts)
            {
                host.Dispose();
            }
        }
    }
}