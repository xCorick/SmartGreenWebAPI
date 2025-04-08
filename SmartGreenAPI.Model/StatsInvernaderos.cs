using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGreenAPI.Model
{
    public class StatsInvernaderos
    {
        [BsonElement("IdInvernadero")]
        public string? idInvernadero { get; set; }
        [BsonElement("stats")]
        public List<DailyStat>? stats { get; set; }
    }
}
