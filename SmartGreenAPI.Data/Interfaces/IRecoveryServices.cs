using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartGreenAPI.Model;
using SmartGreenAPI.Model.DTOs;

namespace SmartGreenAPI.Data.Interfaces
{
    public interface IRecoveryServices
    {
        Task<string> CreateRecoveryTokenAsync(string email);
       
        Task<bool> ValidateTokenAsync(string email, string token);
    
        Task<bool> ChangePasswordAsync(ChangePasswordDto dto);

        //Recuperación por código
        Task<string> CreateRecoveryCodeAsync(string email);
        Task<bool> ValidateCodeAsync(string email, string code);
        Task<bool> ChangePasswordWithCode(ChangePassDto changedto);

    }
}
