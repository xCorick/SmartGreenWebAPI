﻿using Microsoft.Extensions.Options;
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
            var invernadero = new InvernaderoModel
            {
                idInvernadero = id,
                TipoInvernadero = tipo,
                Started = false
            };  
            await _invernadero.InsertOneAsync(invernadero);
            return invernadero;
        }

        public async Task<InvernaderoModel?> RegistrarInvernadero(InvernaderoModel invernadero)
        {
            var findInver = await this.FindById(invernadero.idInvernadero!);

            if (findInver == null || findInver.UsuCorreo != null)
            {
                return null;
            }
            
            invernadero.TipoInvernadero = findInver.TipoInvernadero;
            invernadero.idInvernadero = findInver.idInvernadero;
            invernadero.id = findInver.id;
            invernadero.Started = false;

            var filter = Builders<InvernaderoModel>.Filter.Eq(i => i.idInvernadero, invernadero.idInvernadero);

            var result = await _invernadero.ReplaceOneAsync(filter, invernadero);

            return invernadero;
        }

        public async Task<List<InvernaderoModel>> FindAll() => await _invernadero.Find(_ => true).ToListAsync();

        public async Task<InvernaderoModel?> FindById(string id)
        {
            var filter = Builders<InvernaderoModel>.Filter.Eq(u => u.idInvernadero, id);
            return await _invernadero.Find(filter).FirstOrDefaultAsync();
             
        }

        public async Task DeleteById(string id)
        {
            var filter = Builders<InvernaderoModel>.Filter.Eq(u => u.idInvernadero, id);
            await _invernadero.DeleteOneAsync(filter);
        }

        public async Task<List<InvernaderoModel>> FindByUser(string correo)
        {
            var filter = Builders<InvernaderoModel>.Filter.Eq(u => u.UsuCorreo, correo);
            return await _invernadero.Find(filter).ToListAsync();
        }
    }
}
