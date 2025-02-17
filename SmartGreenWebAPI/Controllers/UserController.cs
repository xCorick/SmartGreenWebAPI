using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SmartGreenAPI.Data.Services;
using SmartGreenAPI.Model;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SmartGreenWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly UserServices _userServices;

        public UserController(ILogger<UserController> logger, UserServices userServices)
        {
            _logger = logger;
            _userServices = userServices;
        }

        // GET: api/<UserController>
        [HttpGet]
        public async Task<IActionResult> FindAll()
        {
            var user = await _userServices.FindAll();
            return Ok(user);
        }

        // GET api/<UserController>/5
        [HttpGet("Correo/{correo}")]
        public async Task<IActionResult> FindByEmail(string correo)
        {
            var user = await _userServices.FindByEmail(correo);
            return Ok(user);
        }

        // POST api/<UserController>
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody]UserModel user)
        {
            if (user == null)
            {
                return BadRequest();
            }

            var result = await _userServices.CreateUser(user);

            return Ok(result);
        }

        // PUT api/<UserController>/5
        [HttpPut("UpdateUser")]
        public async Task<IActionResult> UpdateUser([FromBody]UserModel usermodel)
        {
            if (usermodel.Id == null || usermodel.Id == string.Empty)
            {
                ModelState.AddModelError("Id","El id no debe ir vacio");
            }
            if (usermodel.Correo == null || usermodel.Correo == string.Empty)
            {
                ModelState.AddModelError("Correo", "El correo no debe ir vacio");
            }
            if (usermodel.Nombre == null || usermodel.Nombre == string.Empty)
            {
                ModelState.AddModelError("Nombre", "El nombre no debe ir vacio");
            }
            if (usermodel.Celular == null || usermodel.Celular == string.Empty)
            {
                ModelState.AddModelError("Celular", "El celular no debe ir vacio");
            }
            if (usermodel.Password == null || usermodel.Password == string.Empty)
            {
                ModelState.AddModelError("Password", "La contraseña no debe ir vacia");
            }
            if (usermodel.UsuarioTipo == null || usermodel.UsuarioTipo == string.Empty)
            {
                ModelState.AddModelError("UsuarioTipo", "El tipo de usuario no debe ir vacio");
            }

            var user = await _userServices.UpdateUser(usermodel);
            return Ok(usermodel);
        }

        [HttpPut("ChangePassword/{correo}")]
        public async Task<IActionResult> ChangePassword(string correo, string password)
        {
            await _userServices.ChangePassword(correo,password);

            return Ok();
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{email}")]
        public async Task<IActionResult> DeleteByEmail(string email)
        {
            await _userServices.DeleteByEmail(email);
            return Ok();
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(string correo, string pass)
        {
            var user = await _userServices.FindByEmail(correo);

            if (user == null)
            {
                return BadRequest();
            }

            string? hash = user.Password;
            bool result = BCrypt.Net.BCrypt.Verify(pass, hash);

            if (result)
            {
                return Ok(user);
            }
            ModelState.AddModelError("Password", "La contraseña no coincide");
            return BadRequest();
        }
    }
}
