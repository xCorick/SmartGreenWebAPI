using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGreenAPI.Model.DTOs
{
    public class ValidateCodeDto
    {
        public string Email { get; set; }
        public string Code { get; set; }
    }
}
