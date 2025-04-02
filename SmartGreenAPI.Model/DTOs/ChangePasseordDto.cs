using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGreenAPI.Model.DTOs
{
    public class ChangePasseordDto
    {
        public string Token { get; set; }
        [Required]
        [MinLength(8, ErrorMessage = "La contraseña debe tener mínimo 8 caracteres.")]
        public string NewPassword { get; set; }
    }
}
