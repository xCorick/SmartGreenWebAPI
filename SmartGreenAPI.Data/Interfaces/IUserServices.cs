using SmartGreenAPI.Model;
using SmartGreenAPI.Model.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGreenAPI.Data.Interfaces
{
    public interface IUserServices
    {
        public Task<UserModel> CreateUser(CreateUserDTO createUser);

        public Task<UserModel> UpdateUser(UpdateUserDTO updateUser);

        public Task<List<UserModel>> FindAll();

        public Task<UserModel> FindByEmail(string email);

        public Task DeleteByEmail(string email);

        public Task ChangePassword(string correo,string password);
    }
}
