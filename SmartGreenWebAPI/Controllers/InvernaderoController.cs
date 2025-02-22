using Microsoft.AspNetCore.Mvc;
using SmartGreenAPI.Data.Services;
using SmartGreenAPI.Model;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SmartGreenWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvernaderoController : ControllerBase
    {
        private readonly InvernaderoServices _invernaderoServices;
        private readonly ILogger<InvernaderoController> _logger;

        public InvernaderoController(InvernaderoServices invernaderoServices, ILogger<InvernaderoController> logger)
        {
            _invernaderoServices = invernaderoServices;
            _logger = logger;
        }
        // GET: api/<InvernaderoController>
        [HttpGet("FindAll")]
        public async Task<IActionResult> FindAll()
        {
            var invernadero = await _invernaderoServices.FindAll();
            return Ok(invernadero);
        }

        // GET api/<InvernaderoController>/5
        [HttpGet("Find/{id}")]
        public async Task<IActionResult> FindById(string id)
        {
            var invernadero = await _invernaderoServices.FindById(id); //?? throw new ArgumentNullException(nameof(InvernaderoModel));

            if (invernadero == null)
            {
                return NotFound();
            }

            return Ok(invernadero); 
        }

        [HttpGet("FindByEmail/{correo}")]
        public async Task<IActionResult> FindByEmail(string correo)
        {
            var invernadero = await _invernaderoServices.FindByUser(correo);
            return Ok(invernadero);
        }

        // POST api/<InvernaderoController>
        [HttpPost("Create")]
        public async Task<IActionResult> CreateInvernadeo(string id, int tipo)
        {
            var invernadero = await _invernaderoServices.CreateInvernadero(id, tipo);
            return Ok(invernadero);
        }

        // PUT api/<InvernaderoController>/5
        [HttpPut("RegistrarInvernadero")]
        public async Task<IActionResult> RegistrarInvernadero([FromBody] InvernaderoModel invernadero)
        {
            var inver = await _invernaderoServices.RegistrarInvernadero(invernadero);
            if (inver == null)
            {
                return NotFound();
            }
            return Ok(inver);
        }

        // DELETE api/<InvernaderoController>/5
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteById(string id)
        {
            await _invernaderoServices.DeleteById(id);
            return Ok();
        }
    }
}
