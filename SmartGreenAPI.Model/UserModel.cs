using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGreenAPI.Model
{
    public class UserModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        [BsonElement("Correo")]
        public string? Correo { get; set; }
        [BsonElement("Nombre")]
        public string? Nombre { get; set; }
        [BsonElement("Celular")]
        public string? Celular { get; set; }
        [BsonElement("Password")]
        public string? Password { get; set; }
        [BsonElement("UsuarioTipo")]
        public string? UsuarioTipo  { get; set; }
    }
}
