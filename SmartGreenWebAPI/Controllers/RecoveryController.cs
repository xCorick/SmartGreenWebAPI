using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartGreenAPI.Data.Interfaces;
using SmartGreenAPI.Model.DTOs;

namespace SmartGreenWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecoveryController : ControllerBase
    {
        private readonly IRecoveryServices _recovery;
        public RecoveryController(IRecoveryServices recovery)
        {
            _recovery = recovery;
        }

        [HttpPost("recovery")]
        public async Task<IActionResult> RecoveryRequestAsync([FromBody] string email)
        {
            var token = await _recovery.CreateRecoveryTokenAsync(email);

            if (string.IsNullOrEmpty(token)) throw new Exception("El correo es invalido");
            return Ok(new {Token = token});
        }

        [HttpPost("validate")]
        public async Task<IActionResult>ValidateTokenAsync([FromBody]string token)
        {
            var isValid = await _recovery.ValidateTokenAsync(token);
            if (!isValid) throw new Exception("Token invalido.");
            return Ok();
        }

        [HttpPost("change")]
        public async Task<IActionResult> ChangePasswordAsync([FromBody] ChangePasseordDto dto)
        {
            var response = await _recovery.ChangePasswordAsync(dto.Token, dto.NewPassword);
            if (!response) throw new Exception("No se puedo cambiar la contraseña.");
            return Ok();
        }
    }
}
