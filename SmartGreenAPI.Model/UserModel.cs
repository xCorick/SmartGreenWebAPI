﻿using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;
using System.ComponentModel.DataAnnotations;

namespace SmartGreenAPI.Model
{
    public class UserModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        

        [BsonElement("Correo")]
        [Required(ErrorMessage = "El correo es requerido.")]
        [EmailAddress(ErrorMessage = "El correo no es valido.")]
        public string? Correo { get; set; }
        [BsonElement("Nombre")]
        [Required(ErrorMessage = "El nombre es requerido.")]
        [StringLength(30, ErrorMessage = "El nombre no puede tener mas de 30 caracteres.")]
        public string? Nombre { get; set; }
        [BsonElement("Celular")]
        [Required(ErrorMessage = "El celular es requerido.")]
        [StringLength(10, ErrorMessage = "El celular no puede tener mas de 10 caracteres.")]
        public string? Celular { get; set; }
        [BsonElement("Password")]
        [Required(ErrorMessage = "La contraseña es requerida.")]
        //[StringLength(30, ErrorMessage = "La contraseña no puede tener mas de 30 caracteres.")]
        [MinLength(8, ErrorMessage = "La contraseña debe tener un mínimo de 8 caracteres.")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        [BsonElement("UsuarioTipo")]
        [Required(ErrorMessage = "El tipo de usuario es requerido.")]
        public string? UsuarioTipo  { get; set; }
        [BsonElement("Access")]
        public AccessModel? Access { get; set; }
    }
}
