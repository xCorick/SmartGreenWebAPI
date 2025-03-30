﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGreenAPI.Model
{
    public  class EmailModel
    {
        [Required]
        public string EmailRecipient { get; set; }
        [Required]
        public string Subject {  get; set; }
        [Required]
        public string Body {  get; set; }   
    }
}
