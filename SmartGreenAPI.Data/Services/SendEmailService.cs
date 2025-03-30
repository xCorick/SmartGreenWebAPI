using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using SmartGreenAPI.Data.Interfaces;
using SmartGreenAPI.Model;
namespace SmartGreenAPI.Data.Services
{
   
    public class SendEmailService : ISendEmailService
    {
        private readonly IConfiguration _configuration;
        
        public SendEmailService(IConfiguration configuration)
        {
            this._configuration = configuration;
        }
        public async Task SendEmail(EmailModel emailModel)
        {
            var emailSender = _configuration["EMAIL_CONFIGURATION:EMAIL"];
            var password = _configuration["EMAIL_CONFIGURATION:PASSWORD"];
            var host = _configuration["EMAIL_CONFIGURATION:HOST"];
            var port = _configuration["EMAIL_CONFIGURATION:PORT"];


            if (string.IsNullOrEmpty(emailSender) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(host) || string.IsNullOrEmpty(port))
            {
                throw new ArgumentException("Los valores de la configuración son incorrectos.");
            }

            try
            {
                var smtpClient = new SmtpClient(host, Convert.ToInt32(port))
                {
                    EnableSsl = true,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(emailSender, password)
                };

                var message = new MailMessage(emailSender, emailModel.EmailRecipient, emailModel.Subject, emailModel.Body);

                await smtpClient.SendMailAsync(message);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error al envíar el correo.", ex);
            }
        }
    }
}
