using SmartGreenAPI.Model;
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

        public Task RegistrarInvernadero(string correo, InvernaderoModel invernadero);
    }
}
