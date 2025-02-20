using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SmartGreenAPI.Data.Interfaces;
using SmartGreenAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGreenAPI.Data.Services
{
    public class InvernaderoServices : IInvernaderoServices
    {
        private readonly IMongoCollection<InvernaderoModel> _invernadero;
        private MongoConfiguration _configuration;
        private MongoClient _client;

        public InvernaderoServices(MongoClient mongoClient, IOptions<MongoConfiguration> mongoConfig)
        {
            _client = mongoClient;
            _configuration = mongoConfig.Value ?? throw new ArgumentNullException(nameof(mongoConfig));

            var mongoDB = _client.GetDatabase(_configuration.DataBase);
            _invernadero = mongoDB.GetCollection<InvernaderoModel>("invernaderos");
        }

        public async Task<InvernaderoModel> CreateInvernadero(string id, int tipo)
        {
            throw new NotImplementedException();
        }

        public async Task RegistrarInvernadero(string correo, InvernaderoModel invernadero)
        {
            throw new NotImplementedException();
        }
    }
}
