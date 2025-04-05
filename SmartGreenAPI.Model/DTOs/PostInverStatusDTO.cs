using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace SmartGreenAPI.Model.DTOs
{
    public class PostInverStatusDTO
    {
        public string? idInvernadero { get; set; }
        [BsonElement("CurrentHumedad")]
        public double CurrentHumedad { get; set; }
        [BsonElement("CurrentTemperatura")]
        public double CurrentTemperatura { get; set; }
        [BsonElement("CurrentLuz")]
        public double CurrentLuz { get; set; }
        [BsonElement("maxHumedad")]
        public double maxHumedad { get; set; }
        [BsonElement("minHumedad")]
        public double minHumedad { get; set; }
        [BsonElement("maxTemperatura")]
        public double maxTemperatura { get; set; }
        [BsonElement("minTemperatura")]
        public double minTemperatura { get; set; }
    }
}
