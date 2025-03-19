using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;

namespace SmartGreenAPI.Model.DTOs
{
    public class UpdateUserDTO
    {
        [BsonElement("Correo")]
        [Required(ErrorMessage = "El correo es requerido.")]
        [EmailAddress(ErrorMessage = "El correo no es valido.")]
        public string? Correo { get; set; }
        public string? Nombre { get; set; }
        [BsonElement("Celular")]
        [Required(ErrorMessage = "El celular es requerido.")]
        [StringLength(10, ErrorMessage = "El celular no puede tener mas de 10 caracteres.")]
        public string? Celular { get; set; }
        [BsonElement("Password")]
        [Required(ErrorMessage = "La contraseña es requerida.")]
        public string? UsuarioTipo { get; set; }
    }
}
