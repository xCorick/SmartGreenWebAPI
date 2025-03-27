using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SmartGreenAPI.Data.Services;
using SmartGreenAPI.Model;

using SmartGreenAPI.Model.DTOs;
using SmartGreenWebAPI.Filters;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SmartGreenWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly UserServices _userServices;
        private readonly AuthUserService _authUserService;

        public UserController(ILogger<UserController> logger, UserServices userServices, AuthUserService authUserService)
        {
            _logger = logger;
            _userServices = userServices;
            _authUserService = authUserService;
        }

        //[HttpGet("ErrorPrueba")]
        //public IActionResult GenerateError ()
        //{
        //    throw new Exception("Error generado");
        //}

        // GET: api/<UserController>

        //[Authorize]
        [HttpGet("FindAll")]
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
        [HttpPost("Register")]
        public async Task<IActionResult> CreateUser([FromBody]CreateUserDTO user)
        {
            if (user == null)
            {
                return BadRequest();
            }

            var result = await _userServices.CreateUser(user);

            return Ok(result);
        }

        // PUT api/<UserController>/5
        //[Authorize]
        [HttpPatch("UpdateUser")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDTO updateUser)
        {
            var user = await _userServices.UpdateUser(updateUser);
            return Ok(user);
        }
        [Authorize]
        [HttpPut("ChangePassword/{correo}")]
        public async Task<IActionResult> ChangePassword(string correo, string password)
        {
            await _userServices.ChangePassword(correo,password);

            return Ok();
        }

        // DELETE api/<UserController>/5
        //[Authorize]
        [HttpDelete("{email}")]
        public async Task<IActionResult> DeleteByEmail(string email)
        {
            await _userServices.DeleteByEmail(email);
            return Ok();
        }

        [HttpPost("Login")]

        public async Task<IActionResult> Login([FromBody] RequestLoginDto request)
        {
            var user = await _userServices.FindByEmail(request.Email);

            if (user == null)
            {
                return BadRequest();
            }

            string? hash = user.Password;
            bool result = BCrypt.Net.BCrypt.Verify(request.Password, hash);

            if (!result)
            {
                return BadRequest(new {message = "Contraseña incorrecta"});
            }
            
            var token = _authUserService.GenerateJwtToken(user);
            return Ok(token);
        }
    }
}
