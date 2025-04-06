using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using SmartGreenAPI.Data.Interfaces;
using SmartGreenAPI.Model;

namespace SmartGreenAPI.Data.Services
{
    public class StatsService : IStatsService
    {
        private readonly IMongoCollection<DailyStat> _stats;
        private readonly IMongoCollection<InverStatusModel> _inverStatus;
        private MongoConfiguration _configuration;
        private MongoClient _client;

        public StatsService(MongoClient mongoClient, IOptions<MongoConfiguration> mongoConfig)
        {
            _client = mongoClient;
            _configuration = mongoConfig.Value ?? throw new ArgumentNullException(nameof(mongoConfig));
            var mongoDB = _client.GetDatabase(_configuration.DataBase);

            _stats = mongoDB.GetCollection<DailyStat>("dailyStats");
            _inverStatus = mongoDB.GetCollection<InverStatusModel>("inverStatus");
        }

        public async Task<List<DailyStat>> SetDailyAVG()
        {
            var today = DateTime.UtcNow.Date;
            var tomorrow = today.AddDays(1);

            try
            {
                // 1. Verificar si hay datos primero
                var existsFilter = Builders<InverStatusModel>.Filter.And(
                    Builders<InverStatusModel>.Filter.Gte(x => x.Fecha, today),
                    Builders<InverStatusModel>.Filter.Lt(x => x.Fecha, tomorrow)
                );

                var anyData = await _inverStatus.CountDocumentsAsync(existsFilter) > 0;
                if (!anyData)
                {
                    Console.WriteLine($"No hay datos para el rango de fechas: {today} - {tomorrow}");
                    return new List<DailyStat>();
                }

                // 2. Pipeline de agregación con validación
                var pipeline = new BsonDocument[]
                {
                    new BsonDocument("$match",
                        new BsonDocument
                        {
                            { "Fecha", new BsonDocument
                                {
                                    { "$gte", today },
                                    { "$lt", tomorrow }
                                }
                            },
                            { "IdInvernadero", new BsonDocument("$exists", true) } // Cambiado a "IdInvernadero"
                        }
                    ),
                    new BsonDocument("$group",
                        new BsonDocument
                        {
                            { "_id", "$IdInvernadero" }, // Cambiado a "$IdInvernadero"
                            { "humedadPromedio", new BsonDocument("$avg", "$CurrentHumedad") },
                            { "temperaturaPromedio", new BsonDocument("$avg", "$CurrentTemperatura") },
                            { "luzPromedio", new BsonDocument("$avg", "$CurrentLuz") },
                            { "count", new BsonDocument("$sum", 1) }
                        }
                    ),
                    new BsonDocument("$match",
                        new BsonDocument("count", new BsonDocument("$gt", 0))
                    )
                };

                var results = await _inverStatus.Aggregate<BsonDocument>(pipeline).ToListAsync();
                Console.WriteLine($"Resultados de agregación: {results.ToJson()}"); // Debug

                // 3. Mapeo con validación
                var stats = new List<DailyStat>();
                foreach (var doc in results)
                {
                    try
                    {
                        stats.Add(new DailyStat
                        {
                            idInvernadero = doc["_id"].AsString,
                            humedadPromedio = Math.Round(doc["humedadPromedio"].AsDouble, 1),
                            temperaturaPromedio = Math.Round(doc["temperaturaPromedio"].AsDouble, 1),
                            luzPromedio = Math.Round(doc["luzPromedio"].AsDouble, 1),
                            fecha = today
                        });
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error mapeando documento: {doc.ToJson()}. Error: {ex.Message}");
                    }
                }

                // 4. Guardar solo si hay resultados
                if (stats.Any())
                {
                    await _stats.DeleteManyAsync(Builders<DailyStat>.Filter.Eq(x => x.fecha, today));
                    await _stats.InsertManyAsync(stats);
                }

                return stats;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en SetDailyAVG: {ex.Message}\n{ex.StackTrace}");
                throw;
            }
        }
    }
}
