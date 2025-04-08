using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Newtonsoft.Json.Linq;
using SmartGreenAPI.Data.Interfaces;
using SmartGreenAPI.Model;
using SmartGreenAPI.Model.DTOs;

namespace SmartGreenAPI.Data.Services
{
    public class RecoveryService : IRecoveryServices
    {
        private readonly IMemoryCache _memory;
        private readonly IMongoCollection<UserModel> _users;
        private MongoConfiguration _configuration;
        private MongoClient _client;
        private readonly AuthUserService _auth;
        public RecoveryService(IMemoryCache memory, MongoClient mongoClient, IOptions<MongoConfiguration> mongoConfig, AuthUserService auth) {
            _auth = auth;
            _memory = memory;
            _client = mongoClient;
            _configuration = mongoConfig.Value ?? throw new ArgumentNullException(nameof(mongoConfig));

            var mongoDB = _client.GetDatabase(_configuration.DataBase);
            _users = mongoDB.GetCollection<UserModel>("usuarios");
        }

        public async Task<string> CreateRecoveryTokenAsync(string email)
        {
            var user = await _users.Find(u => u.Correo == email).FirstOrDefaultAsync();
            if (user == null) return null;
            try
            {
                var token = _auth.GenerateRecoveryToken(email);
                var data = new Dictionary<string, object>
                {
                    {"Email", email },
                    { "Token", token },
                    { "HoraGeneracion", DateTime.Now }
                };
                _memory.Set(email, data, TimeSpan.FromMinutes(10));
                return token;

            }
            catch (Exception ex)
            {

                return "Error: " + ex.Message;
            }
        }
        //public async Task<bool> ValidateTokenAsync(string token)
        //{
        //    var email = _memory.Get<string>(token);
        //    if (string.IsNullOrEmpty(email)) return false;

        //    return true;
        //}

        public async Task<bool> ValidateTokenAsync(string email, string token)
        {
            if (_memory.TryGetValue(email, out Dictionary<string, object> data))
            {
                // Obtener el código y la hora de generación desde el Dictionary
                var storedCode = data["Token"].ToString();
                var horaGeneracion = (DateTime)data["HoraGeneracion"];

                if (storedCode == token)
                {
                    var tiempoValidez = DateTime.Now - horaGeneracion;

                    if (tiempoValidez.TotalMinutes <= 10)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
        public async Task<bool> ChangePasswordAsync(ChangePasswordDto change)
        {
            if (!IsPasswordValid(change.NewPassword)) return false;
            if (string.IsNullOrEmpty(change.Token) || string.IsNullOrEmpty(change.NewPassword) || string.IsNullOrEmpty(change.ConfirmPassword)) return false;
            if (change.NewPassword != change.ConfirmPassword) return false; // Las contraseñas no coinciden
            if (!IsPasswordValid(change.NewPassword)) return false;

            var isValidCode = await ValidateTokenAsync(change.Email, change.Token);
            if (!isValidCode) return false;

            var user = await _users.Find(u => u.Correo == change.Email).FirstOrDefaultAsync();
            if (user == null) return false;

            string passHashed = BCrypt.Net.BCrypt.HashPassword(change.NewPassword);

            var filter = Builders<UserModel>.Filter.Eq(u => u.Correo, change.Email);
            var update = Builders<UserModel>.Update.Set(u => u.Password, passHashed);
            var result = await _users.UpdateOneAsync(filter, update);

            if (result.ModifiedCount > 0)
            {
                _memory.Remove(change.Token);
                return true;
            }

            return false;
        }
        //Recuperación de contraseña con código
        public async Task<string> CreateRecoveryCodeAsync(string email)
        {
            var user = await _users.Find(u => u.Correo == email).FirstOrDefaultAsync();
            if (user == null) return null;

            var code = new Random().Next(100000, 999999).ToString();

            // Almacenar el código y la hora de generación en un Dictionary
            var data = new Dictionary<string, object>
            {
                {"Email", email },
                { "Codigo", code },
                { "HoraGeneracion", DateTime.Now }
            };

            _memory.Set(email, data, TimeSpan.FromMinutes(10)); 

            return code; 
        }
        public async Task<bool> ValidateCodeAsync(string email, string code)
        {
            if (_memory.TryGetValue(email, out Dictionary<string, object> data))
            {
                // Obtener el código y la hora de generación desde el Dictionary
                var storedCode = data["Codigo"].ToString();
                var horaGeneracion = (DateTime)data["HoraGeneracion"];

                if (storedCode == code)
                {
                    var tiempoValidez = DateTime.Now - horaGeneracion;

                    if (tiempoValidez.TotalMinutes <= 10)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
        public async Task<bool> ChangePasswordWithCode(ChangePassDto change)
        {
            if (!IsPasswordValid(change.NewPassword)) return false;
            if (string.IsNullOrEmpty(change.NewPassword) || string.IsNullOrEmpty(change.ConfirmPassword)) return false;
            if (change.NewPassword != change.ConfirmPassword) return false; // Las contraseñas no coinciden
            if (!IsPasswordValid(change.NewPassword)) return false;

            var isValidCode = await ValidateCodeAsync(change.Email, change.Code);
            if (!isValidCode) return false;

            var user = await _users.Find(u => u.Correo == change.Email).FirstOrDefaultAsync();
            if (user == null) return false;

            string passHashed = BCrypt.Net.BCrypt.HashPassword(change.NewPassword);

            var filter = Builders<UserModel>.Filter.Eq(u => u.Correo, change.Email);
            var update = Builders<UserModel>.Update.Set(u => u.Password, passHashed);
            var result = await _users.UpdateOneAsync(filter, update);

            if (result.ModifiedCount > 0)
            {
                _memory.Remove(change.Code);
                return true;
            }

            return false;
        }

       //Validaciones
        private bool IsPasswordValid(string password)
        {
            // Verificar que la contraseña tenga al menos un número, una letra mayúscula y un carácter especial
            var hasUpperCase = password.Any(char.IsUpper);
            var hasLowerCase = password.Any(char.IsLower);
            var hasNumber = password.Any(char.IsDigit);
            var hasSpecialChar = password.Any(ch => !char.IsLetterOrDigit(ch));

            return hasUpperCase && hasLowerCase && hasNumber && hasSpecialChar;
        }
        //public bool VerifyRecoveryCode(string email, string enteredCode)
        //{
        //    if (_memory.TryGetValue(email, out string storedCode))
        //    {
        //        if (storedCode == enteredCode) return true;
        //    }    
        //    return false;
        //}

    }
}