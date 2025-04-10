﻿using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using SmartGreenAPI.Data.Interfaces;
using SmartGreenAPI.Model;
using SmartGreenAPI.Model.DTOs;
using System;
using System.Collections.Generic;
using System.Data.Common;
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

        public async Task<InvernaderoModel> RegistrarInvernadero(RequestRegistrarInvernaderoDto regInvernadero)
        {
            var filter = Builders<InvernaderoModel>.Filter.Eq(i => i.idInvernadero, regInvernadero.IdInvernadero);

            var updateInvernadero = Builders<InvernaderoModel>.Update
            .Set(i => i.UsuCorreo, regInvernadero.UsuCorreo)
            .Set(i => i.NombreInvernadero, regInvernadero.NombreInvernadero)
            .Set(i => i.Descripcion, regInvernadero.Descripcion);

            var result = await  _invernadero.UpdateOneAsync(filter, updateInvernadero);

            if (result.MatchedCount == 0)
            {
                throw new Exception("El invernadero no existe.");
            }
            return await _invernadero.Find(filter).FirstOrDefaultAsync();

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
        public async Task<int> ToggleStatus(string id)
        {
            var filter = Builders<InvernaderoModel>.Filter.Eq(u => u.idInvernadero, id);

            var projection = Builders<InvernaderoModel>.Projection.Include(u => u.Started);

            var result = await _invernadero.Find(filter).Project<BsonDocument>(projection).FirstOrDefaultAsync();

            if (result == null || !result.Contains("Started"))
            {
                return -1;
            }
            bool started = result["Started"].AsBoolean;
            bool newStatus = !started;

            var update = Builders<InvernaderoModel>.Update.Set(u => u.Started, newStatus);
            var updateResult = await _invernadero.UpdateOneAsync(filter, update);

            if (updateResult.ModifiedCount == 0)
            {
                return -1;
            }
            return 1;
        }
        public async Task<InvernaderoModel> ChangeParameters(ChangeInverParameters parameters)
        {
            var filter = Builders<InvernaderoModel>.Filter.Eq(u => u.idInvernadero, parameters.idInvernadero);
            var update = Builders<InvernaderoModel>.Update
            .Set(u => u.MinHumedad, parameters.MinHumedad)
            .Set(u => u.MaxHumedad, parameters.MaxHumedad)
            .Set(u => u.MinTemperatura, parameters.MinTemperatura)
            .Set(u => u.MaxTemperatura, parameters.MaxTemperatura);
            var result = await _invernadero.UpdateOneAsync(filter, update);
            if (result.MatchedCount == 0)
            {
                throw new Exception("El invernadero no existe.");
            }
            return await _invernadero.Find(filter).FirstOrDefaultAsync();
        }
    }
}
