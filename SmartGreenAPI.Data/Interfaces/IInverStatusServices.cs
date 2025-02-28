using SmartGreenAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGreenAPI.Data.Interfaces
{
    public interface IInverStatusServices
    {
        public Task<InverStatusModel> GetLastInverStatusById(string idInvernadero);

        public Task<List<InverStatusModel>> GetAllInverStatusById(string idInvernadero);

        public Task<InverStatusModel> PostInverStatus(InverStatusModel inverStatus);
    }
}
