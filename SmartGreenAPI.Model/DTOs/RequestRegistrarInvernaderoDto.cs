using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGreenAPI.Model.DTOs
{
    public class RequestRegistrarInvernaderoDto
    {
        [Required]
        public string IdInvernadero { get; set; }

        [Required]
        public string UsuCorreo { get; set; }
        [Required]
        public string NombreInvernadero { get; set; }
        [Required]
        public string Descripcion { get; set; }

    }
}
