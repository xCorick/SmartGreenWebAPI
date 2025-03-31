using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;

namespace SmartGreenAPI.Model
{
    public class AccessModel
    {
        [BsonElement("RecoveryToken")]
        public string RecoveryToken { get; set; } = "";
        [BsonElement("CreateDate")]
        public DateTime? CreateDate { get; set; }
        [BsonElement("ExpDate")]
        public DateTime? ExpDate { get; set; }
    }
}
