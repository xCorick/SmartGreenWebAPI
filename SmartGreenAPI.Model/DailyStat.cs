using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGreenAPI.Model
{
    public class DailyStat
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        string id { get; set; } = string.Empty;
        [BsonElement("IdInvernadero")]
        public string? idInvernadero { get; set; }
        [BsonElement("HumedadPromedio")]
        public double humedadPromedio { get; set; }
        [BsonElement("TemperaturaPromedio")]
        public double temperaturaPromedio { get; set; }
        [BsonElement("LuzPromedio")]
        public double luzPromedio { get; set; }
        [BsonElement("Fecha")]
        public DateTime fecha { get; set; }
    }
}
