using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGreenAPI.Model
{
    public class InvernaderoModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? id { get; set; }
        [BsonElement("IdInvernadero")]
        public string? idInvernadero { get; set; }
        [BsonElement("UsuCorreo")]
        public string? UsuCorreo { get; set; }
        [BsonElement("NombreInvernadero")]
        public string? NombreInvernadero { get; set; }
        [BsonElement("Descripcion")]
        public string? Descripcion { get; set; } = "";
        [BsonElement("TipoInvernadero")]
        public int TipoInvernadero { get; set; }
        [BsonElement("MinHumedad")]
        public double MinHumedad { get; set; }
        [BsonElement("MaxHumedad")]
        public double MaxHumedad { get; set; }
        [BsonElement("MinTemperatura")]
        public double MinTemperatura { get; set; }
        [BsonElement("MaxTemperatura")]
        public double MaxTemperatura { get; set; }
        [BsonElement("Started")]
        public bool Started { get; set; }
    }
}
