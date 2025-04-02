using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartGreenAPI.Data;

using SmartGreenAPI.Model;
namespace SmartGreenAPI.Data.Interfaces
{
    public interface ISendEmailService
    {
        Task SendEmail(EmailModel emailModel);

        Task SendEmailAsync(string token, string email);
    }
}
