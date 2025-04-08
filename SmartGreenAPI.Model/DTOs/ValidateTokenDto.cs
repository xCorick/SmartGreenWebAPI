using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGreenAPI.Model.DTOs
{
    public class ValidateTokenDto
    {
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
