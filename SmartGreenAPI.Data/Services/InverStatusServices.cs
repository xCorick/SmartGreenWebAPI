﻿using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SmartGreenAPI.Data.hub;
using SmartGreenAPI.Data.Interfaces;
using SmartGreenAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGreenAPI.Data.Services
{
    public class InverStatusServices : IInverStatusServices
    {
        private readonly IMongoCollection<InverStatusModel> _inverStatus;
        private MongoConfiguration _configuration;
        private MongoClient _client;
        private readonly IHubContext<InverStatusHub> _hubContext;

        public InverStatusServices(MongoClient mongoClient, IOptions<MongoConfiguration> mongoConfig, IHubContext<InverStatusHub> hubContext)
        {
            _client = mongoClient;
            _configuration = mongoConfig.Value ?? throw new ArgumentNullException(nameof(mongoConfig));
            var mongoDB = _client.GetDatabase(_configuration.DataBase);
            _inverStatus = mongoDB.GetCollection<InverStatusModel>("inverStatus");
            _hubContext = hubContext;
        }
        public Task<List<InverStatusModel>> GetAllInverStatusById(string idInvernadero) => _inverStatus.Find(_ => true).ToListAsync();

        public async Task<InverStatusModel> GetLastInverStatusById(string idInvernadero)
        {
            var result = await _inverStatus.
                Find<InverStatusModel>(i => i.idInvernadero == idInvernadero).
                SortByDescending(i => i.Fecha).
                Limit(1).FirstOrDefaultAsync();
            return result;
        }

        public async Task<InverStatusModel> PostInverStatus(InverStatusModel inverStatus)
        {
            await _inverStatus.InsertOneAsync(inverStatus);

            await _hubContext.Clients.Group(inverStatus.idInvernadero!)
                .SendAsync("ReceiveStatus", inverStatus);

            return inverStatus;
        }
    }
}
