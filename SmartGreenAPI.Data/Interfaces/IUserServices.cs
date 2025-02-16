using SmartGreenAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGreenAPI.Data.Interfaces
{
    public interface IUserServices
    {
        public Task<UserModel> CreateUser(UserModel createUser);

        public Task<UserModel> UpdateUser(UserModel updateUser);

        public Task<List<UserModel>> FindAll();

        public Task<UserModel> FindByEmail(string email);

        public Task DeleteByEmail(string email);

        public Task ChangePassword(string correo,string password);
    }
}
