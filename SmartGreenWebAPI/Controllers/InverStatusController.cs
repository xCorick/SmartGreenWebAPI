using Microsoft.AspNetCore.Mvc;
using SmartGreenAPI.Data.Services;
using SmartGreenAPI.Model;
using SmartGreenAPI.Model.DTOs;
using System.Runtime.CompilerServices;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SmartGreenWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InverStatusController : ControllerBase
    {
        private readonly InverStatusServices _inverStatusServices;
        private readonly ILogger<InverStatusController> _logger;

        public InverStatusController(InverStatusServices inverStatusServices, ILogger<InverStatusController> logger)
        {
            _inverStatusServices = inverStatusServices;
            _logger = logger;
        }

        // GET api/<InverStatusController>/5
        [HttpGet("/GetStatus/{id}")]
        public async Task<IActionResult> GetAllInverStatusById(string id)
        {
            var status = await _inverStatusServices.GetAllInverStatusById(id);
            return Ok(status);
        }

        [HttpGet("/GetLastStatus/{id}")]
        public async Task<IActionResult> GetLastInverStatusById(string id)
        {
            var status = await _inverStatusServices.GetLastInverStatusById(id);
            return Ok(status);
        }

        // POST api/<InverStatusController>
        [HttpPost]
        public async Task<IActionResult> PostInverStatus([FromBody] PostInverStatusDTO inverStatus)
        {
            var status = await _inverStatusServices.PostInverStatus(inverStatus);

            if (status != null)
            {
                return Ok();
            }
            return BadRequest(status);
        }
    }
}
