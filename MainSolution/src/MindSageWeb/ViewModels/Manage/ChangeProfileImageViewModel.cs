﻿using Microsoft.AspNet.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MindSageWeb.ViewModels.Manage
{
    public class ChangeProfileImageViewModel
    {
        [Required]
        [DataType(DataType.Url)]
        [Display(Name = "Profile's image")]
        public IFormFile ImagePath { get; set; }
        public string ClassRoom { get; set; }
    }
}
