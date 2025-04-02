using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SmartGreenAPI.Data.Interfaces;
using SmartGreenAPI.Model;

namespace SmartGreenAPI.Data.Services
{
    public class RecoveryService : IRecoveryServices
    {
        private readonly IMemoryCache _memory;
        private readonly IMongoCollection<UserModel> _users;
        private MongoConfiguration _configuration;
        private MongoClient _client;
        public RecoveryService(IMemoryCache memory, MongoClient mongoClient, IOptions<MongoConfiguration> mongoConfig) {
            
            _memory = memory;
            _client = mongoClient;
            _configuration = mongoConfig.Value ?? throw new ArgumentNullException(nameof(mongoConfig));

            var mongoDB = _client.GetDatabase(_configuration.DataBase);
            _users = mongoDB.GetCollection<UserModel>("usuarios");
        }   
        
        public async Task <string> CreateRecoveryTokenAsync(string email)
        {
            var user = await _users.Find(u => u.Correo == email).FirstOrDefaultAsync();

            if (user == null) return null;

            var token =  Guid.NewGuid().ToString();

            //Guarda el token en la cache con tiempo de expiracion
            _memory.Set(token, email, TimeSpan.FromMinutes(30));

            return token;
        }

        public async Task<bool>ValidateTokenAsync(string token)
        {
            return _memory.TryGetValue(token, out _);
        }

        public async Task<bool> ChangePasswordAsync(string token, string newPassword)
        {
            if (_memory.TryGetValue(token, out string email))
                {
                var user = await _users.Find(u => u.Correo == email).FirstOrDefaultAsync();
                if (user != null)
                {
                    string passHashed = BCrypt.Net.BCrypt.HashPassword(newPassword);
                    var filter = Builders<UserModel>.Filter.Eq(u => u.Correo, email);
                    var update = Builders<UserModel>.Update.Set(u => u.Password, passHashed);
                    await _users.UpdateOneAsync(filter, update);

                    _memory.Remove(token);
                    return true;
                }
            }
            return false;
        }
    }
}
