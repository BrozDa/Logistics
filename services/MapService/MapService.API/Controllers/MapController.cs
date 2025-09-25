using MapService.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MapService.API.Controllers
{
    [ApiController]
    [Route("/api/maps")]
    public class MapController
        (ILogger<MapController> logger,
        IMapRepository mapRepository) : ControllerBase
    {

        private readonly ILogger<MapController> _logger = logger;
        private readonly IMapRepository _repository = mapRepository;

        [HttpGet("health-check")]
        public IActionResult HealthCheck()
        {
            return Ok();
        }
        [HttpGet("map")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _repository.GetAllAsync());
        }
    }
}
