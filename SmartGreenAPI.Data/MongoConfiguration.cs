using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGreenAPI.Data
{
    public class MongoConfiguration
    {
        public string Connection { get; set; } = "";
        public string DataBase { get; set; } = "";
        public string Collection { get; set; } = "";
    }
}
