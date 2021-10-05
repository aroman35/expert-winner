using Microsoft.AspNetCore.Mvc;

namespace WebApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ServersController : ControllerBase
    {
        private readonly HostsManager _hostsManager;

        public ServersController(HostsManager hostsManager)
        {
            _hostsManager = hostsManager;
        }

        [HttpPost]
        public IActionResult CreateRandomHost()
        {
            var hostId = _hostsManager.AddHost();
            return Ok(hostId.ToString("N"));
        }
    }
}