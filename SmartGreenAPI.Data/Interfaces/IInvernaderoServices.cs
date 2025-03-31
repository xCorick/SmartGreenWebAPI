using SmartGreenAPI.Model;
using SmartGreenAPI.Model.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGreenAPI.Data.Interfaces
{
    public interface IInvernaderoServices
    {
        public Task<InvernaderoModel> CreateInvernadero(string id, int tipo);

        public Task<InvernaderoModel> RegistrarInvernadero(RequestRegistrarInvernaderoDto invernadero);

        public Task<List<InvernaderoModel>> FindAll();

        public Task<InvernaderoModel> FindById(string id);

        public Task DeleteById(string id);

        public Task<List<InvernaderoModel>> FindByUser(string id);
        public Task<int> ToggleStatus(string id);

        public Task<InvernaderoModel> ChangeParameters(ChangeInverParameters parameters);
    }
}
