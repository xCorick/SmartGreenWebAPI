using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using MongoDB.Driver;
using SmartGreenAPI.Data.Interfaces;
using SmartGreenAPI.Model.DTOs;

namespace SmartGreenWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecoveryController : ControllerBase
    {
        private readonly IRecoveryServices _recovery;
        private readonly ISendEmailService _send;
        private readonly IMemoryCache _memory;

        public RecoveryController(IRecoveryServices recovery, ISendEmailService send, IMemoryCache memory)
        {
            _recovery = recovery;
            _send = send;
            _memory = memory;
        }
        public class InvalidTokenException : Exception
        {
            public InvalidTokenException(string message) : base(message) { }
        }

        [HttpPost("emailrequest")]
        public async Task<IActionResult> RecoveryRequestAsync([FromBody]  string email)
        {
            try
            { 

                var token = await _recovery.CreateRecoveryTokenAsync(email);
                if (string.IsNullOrEmpty(token)) return StatusCode(500, new { message = "Error al generar el token de recuperación." });
                
                await _send.SendEmailAsync(email, token);

                return Ok( 
                    new { Token = token, message = "Se ha enviado un correo para recuperar la contraseña.", });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor.", details = ex.Message });
            }
        }

        [HttpPost("validatetoken")]
        public async Task<IActionResult> ValidateTokenAsync([FromBody] ValidateTokenDto request)
        {
            try
            {
                var isValid = await _recovery.ValidateTokenAsync(request.Email, request.Token);

                if (!isValid)
                    return BadRequest(new { message = "Token inválido o ha expirado." });

                return Ok(new { message = "Token válido." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno al validar el código.", details = ex.Message });
            }
        }

        //[HttpPost("validate")]
        //public async Task<IActionResult> ValidateTokenAsync([FromBody] string token)
        //{
        //    try
        //    {
        //        var isValid = await _recovery.ValidateTokenAsync(token);
        //        if (!isValid) throw new InvalidTokenException("Token inválido.");
        //        return Ok();
        //    }
        //    catch (InvalidTokenException ex)
        //    {
        //        return BadRequest(new { message = ex.Message });
        //    }
        //    catch (Exception ex)
        //    {        
        //        return StatusCode(500, new { message = "Ha ocurrido un error interno.", details = ex.Message });
        //    }
        //}


        // [HttpPost("recovery")]
        // public async Task<IActionResult> ShowRecoveryPage(string token)
        // {
        //     var email = _memory.Get<string>(token);

        //     if (string.IsNullOrEmpty(email)) return BadRequest("Token inválido o expirado.");

        //     return Ok(new { Email = email });
        // }

        
        [HttpPost("change")]
        //[Authorize]
        public async Task<IActionResult> ChangePasswordAsync([FromBody] ChangePasswordDto dto)
        {
            try
            {
                var response = await _recovery.ChangePasswordAsync(dto);
                if (!response) return BadRequest(new { message = "No se pudo cambiar la contraseña. Asegúrese de que el token sea válido." });

                return Ok(new { message = "Contraseña cambiada con éxito." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno al cambiar la contraseña.", details = ex.Message });
            }
        }

        //[HttpPost("validatetoken")]
        //public async Task<IActionResult> ValidateTokenAsync([FromBody] ValidateCodeDto request)
        //{
        //    try
        //    {
        //        var isValid = await _recovery.ValidateTokenAsync(request.Email, request.Code);

        //        if (!isValid)
        //            return BadRequest(new { message = "Código inválido o ha expirado." });

        //        return Ok(new { message = "Código válido." });
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new { message = "Error interno al validar el código.", details = ex.Message });
        //    }
        //}

        //Recuperación por código
        [HttpPost("coderequest")]
        public async Task<IActionResult> RecoveryRequestCodeAsync([FromBody] string email)
        {
            try
            {
                var code = await _recovery.CreateRecoveryCodeAsync(email);

                if (string.IsNullOrEmpty(code))
                    return StatusCode(500, new { message = "Error al generar el código de recuperación." });

                await _send.SendEmailWithCode(email, code);

                return Ok(new { message = "Se ha enviado un correo para recuperar la contraseña." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor.", details = ex.Message });
            }
        }

        [HttpPost("validatecode")]
        public async Task<IActionResult> ValidateCodeAsync([FromBody] ValidateCodeDto request)
        {
            try
            {
                var isValid = await _recovery.ValidateCodeAsync(request.Email, request.Code);

                if (!isValid)
                    return BadRequest(new { message = "Código inválido o ha expirado." });

                return Ok(new { message = "Código válido." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno al validar el código.", details = ex.Message });
            }
        }

        [HttpPost("changewithcode")]
        public async Task<IActionResult> ChangePasswordWithCodeAsync([FromBody] ChangePassDto dto)
        {
            try
            {
                var response = await _recovery.ChangePasswordWithCode(dto);
                if (!response)
                    return BadRequest(new { message = "No se pudo cambiar la contraseña. Asegúrese de que el código sea válido." });
                return Ok(new { message = "Contraseña cambiada con éxito." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno al cambiar la contraseña.", details = ex.Message });
            }
        }

    }
}
