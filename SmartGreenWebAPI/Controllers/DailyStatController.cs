using Microsoft.AspNetCore.Mvc;
using SmartGreenAPI.Data.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SmartGreenWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DailyStatController : ControllerBase
    {
        private readonly StatsService _statsService;
        private readonly ILogger<DailyStatController> _logger;

        public DailyStatController(StatsService statsService, ILogger<DailyStatController> logger)
        {
            _statsService = statsService;
            _logger = logger;
        }

        // GET: api/<DailyStatController>
        [HttpGet("GetAllStats")]
        public async Task<IActionResult> FindAllStats()
        {
            var result = await _statsService.FindAllStats();
            return Ok(result);
        }

        // POST api/<DailyStatController>
        [HttpPost("PostDailyStadistic")]
        public async Task<IActionResult> PostDailyStadistic()
        {
            var respuesta = await _statsService.SetDailyAVG();

            return Ok(respuesta);
        }
    }
}
