using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;

namespace SmartGreenAPI.Model.DTOs
{
    public class ChangeInverParameters
    {
        [BsonElement("IdInvernadero")]
        public string? idInvernadero { get; set; }
        [BsonElement("MinHumedad")]
        public double MinHumedad { get; set; }
        [BsonElement("MaxHumedad")]
        public double MaxHumedad { get; set; }
        [BsonElement("MinTemperatura")]
        public double MinTemperatura { get; set; }
        [BsonElement("MaxTemperatura")]
        public double MaxTemperatura { get; set; }
    }
}
