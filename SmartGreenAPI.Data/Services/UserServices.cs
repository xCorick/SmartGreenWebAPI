using BCrypt;
using MongoDB.Driver;
using SmartGreenAPI.Data.Interfaces;
using SmartGreenAPI.Model;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace SmartGreenAPI.Data.Services
{
    public class UserServices : IUserServices
    {
        private readonly IMongoCollection<UserModel> _users;
        private MongoConfiguration _configuration;
        private MongoClient _client;

        public UserServices(MongoClient mongoClient, IOptions<MongoConfiguration> mongoConfig)
        {
            _client = mongoClient;
            _configuration = mongoConfig.Value ?? throw new ArgumentNullException(nameof(mongoConfig));

            var mongoDB = _client.GetDatabase(_configuration.DataBase);
            _users = mongoDB.GetCollection<UserModel>("usuarios");
        }
        public async Task<UserModel> CreateUser(UserModel createUser)
        {
            createUser.Password = BCrypt.Net.BCrypt.HashPassword(createUser.Password);
            await _users.InsertOneAsync(createUser);
            return createUser;
        }

        public async Task<UserModel> UpdateUser(UserModel updateUser)
        {
            var filter = Builders<UserModel>.Filter.Eq(u => u.Id, updateUser.Id);
            updateUser.Password = BCrypt.Net.BCrypt.HashPassword(updateUser.Password);
            var result = await _users.ReplaceOneAsync(filter, updateUser);

            if (result.MatchedCount == 0)
            {
                throw new Exception("Usuario no encontrado");
            }
            return updateUser;
        }

        public async Task<List<UserModel>> FindAll() => await _users.Find(_ => true).ToListAsync(); 

        public async Task<UserModel> FindByEmail(string email)
        {
            var filter = Builders<UserModel>.Filter.Eq(u => u.Correo, email);
            var user = await _users.Find(filter).FirstAsync() ?? throw new Exception("El correo no esta registrado");
            return user;
        }

        public async Task DeleteByEmail(string email)
        {
            var filter = Builders<UserModel>.Filter.Eq(u => u.Correo, email);
            await _users.DeleteOneAsync(filter);
        }

        public async Task ChangePassword(string correo, string password)
        {
            string passHashed = BCrypt.Net.BCrypt.HashPassword(password);
            //var user = await this.FindByEmail(correo);
            var filter = Builders<UserModel>.Filter.Eq(u => u.Correo, correo);
            var update = Builders<UserModel>.Update.Set(u => u.Password, passHashed);

            await _users.UpdateOneAsync(filter, update);
        }
    }
}
