using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartGreenAPI.Data;

using SmartGreenAPI.Model;
using SmartGreenAPI.Model.DTOs;
namespace SmartGreenAPI.Data.Interfaces
{
    public interface ISendEmailService
    {
        Task SendEmailWithCode(string email, string code);

        Task SendEmailAsync(string token, string email);
    }
}
