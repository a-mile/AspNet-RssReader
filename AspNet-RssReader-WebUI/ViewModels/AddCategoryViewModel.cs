﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AspNet_RssReader_WebUI.ViewModels
{
    public class AddCategoryViewModel
    {
        [Required]
        [Display(Name = "Category name")]
        [StringLength(15)]
        public string Name { get; set; }
    }
}