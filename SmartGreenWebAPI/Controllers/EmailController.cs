using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartGreenAPI.Data.Interfaces;
using SmartGreenAPI.Model;

namespace SmartGreenWebAPI.Controllers
{
    [Route("api/email")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly ISendEmailService _sendEmailService;

        public EmailController(ISendEmailService sendEmailService)
        {
            _sendEmailService = sendEmailService;
        }

        [HttpPost]
        public async Task<ActionResult>Send(EmailModel emailModel)
        {
            await _sendEmailService.SendEmail(emailModel);
            return Ok();
        }
    }
}
