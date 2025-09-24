using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace MapService.API.Controllers
{
    [ApiController]
    [Route("/api/map")]
    public class MapServiceController
        (ILogger<MapServiceController> logger) : ControllerBase
    {

        private readonly ILogger<MapServiceController> _logger = logger;

        [HttpGet("health-check")]
        public IActionResult HealthCheck()
        {
            return Ok();
        }

        [HttpGet("map")]
        [HttpGet("nodes")]
        [HttpGet("edges")]
    }
}
