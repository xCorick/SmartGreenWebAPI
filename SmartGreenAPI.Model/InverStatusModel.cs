using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGreenAPI.Model
{
    public class InverStatusModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? id { get; set; }
        [BsonElement("IdInvernadero")]
        public string? idInvernadero { get; set; }
        [BsonElement("CurrentHumedad")]
        public double CurrentHumedad { get; set; }
        [BsonElement("CurrentTemperatura")]
        public double CurrentTemperatura { get; set; }
        [BsonElement("CurrentLuz")]
        public double CurrentLuz { get; set; }
        [BsonElement("Fecha")]
        public DateTime Fecha { get; set; } = DateTime.UtcNow;
    }
}
