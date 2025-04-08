using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
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

        public async Task SendEmailAsync(string email, string token)
        {
            var emailSender = _configuration["EMAIL_CONFIGURATION:EMAIL"];
            var password = _configuration["EMAIL_CONFIGURATION:PASSWORD"];
            var host = _configuration["EMAIL_CONFIGURATION:HOST"];
            var port = _configuration["EMAIL_CONFIGURATION:PORT"];
            var subject = "RECUPERACIÓN DE CONTRASEÑA."; 

            try
            {
                var smtpClient = new SmtpClient(host, Convert.ToInt32(port))
                {
                    EnableSsl = true,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(emailSender, password)
                };

                //var url = $"https://smart-green-angular-p7d8.vercel.app/recovery2?token={token}";
                var url = $"http://localhost:4200/recovery2?token={token}";
                var body = $"Para continuar con la recuperación de tu contraseña, haz clic en el siguiente enlace: <a href=\"{url}\">Recuperar mi contraseña</a>";


                var message = new MailMessage
                {
                    From = new MailAddress(emailSender),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true 
                };
                message.To.Add(email);
                await smtpClient.SendMailAsync(message);

                
            }
            catch(Exception ex) 
            {
                throw new Exception("Error al envíar el correo.");
            }
        }


        public async Task SendEmailWithCode(string email, string code)
        {
            var emailSender = _configuration["EMAIL_CONFIGURATION:EMAIL"];
            var password = _configuration["EMAIL_CONFIGURATION:PASSWORD"];
            var host = _configuration["EMAIL_CONFIGURATION:HOST"];
            var port = _configuration["EMAIL_CONFIGURATION:PORT"];
            var subject = "RECUPERACIÓN DE CONTRASEÑA.";

            try
            {
                var smtpClient = new SmtpClient(host, Convert.ToInt32(port))
                {
                    EnableSsl = true,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(emailSender, password)
                };

          
                var body = $"Tú código de validación es:\n {code}";


                var message = new MailMessage
                {
                    From = new MailAddress(emailSender),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };
                message.To.Add(email);
                await smtpClient.SendMailAsync(message);


            }
            catch (Exception ex)
            {
                throw new Exception("Error al envíar el correo.");
            }
        }

        private string GenerateVerificationCode()
        {
            var random = new Random();
            var code = random.Next(100000, 999999); // Genera un código de 6 dígitos
            return code.ToString();
        }
    }
}
