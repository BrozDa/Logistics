using MapService.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MapService.API.Controllers
{
    [ApiController]
    [Route("/api/map-service/edges")]
    public class EdgeController
        (ILogger<EdgeController> logger,
        IEdgeRepository mapRepository) : ControllerBase
    {

        private readonly ILogger<EdgeController> _logger = logger;
        private readonly IEdgeRepository _repository = mapRepository;

        [HttpGet("health-check")]
        public IActionResult HealthCheck()
        {
            return Ok();
        }
        [HttpGet("/")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _repository.GetAllAsync());
        }
    }
}
}
