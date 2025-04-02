using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGreenAPI.Data.Interfaces
{
    public interface IRecoveryServices
    {
        Task<string> CreateRecoveryTokenAsync(string email);
        Task<bool> ValidateTokenAsync(string token);
        Task<bool> ChangePasswordAsync(string token, string newPassword);

    }
}
