using MapService.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MapService.API.Controllers
{
    [ApiController]
    [Route("/api/map-service/nodes")]

    public class NodeController
        (ILogger<NodeController> logger,
        INodeRepository mapRepository) : ControllerBase
    {

        private readonly ILogger<NodeController> _logger = logger;
        private readonly INodeRepository _repository = mapRepository;

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
