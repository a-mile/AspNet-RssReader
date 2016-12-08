﻿using System.ComponentModel.DataAnnotations;

namespace AspNet_RssReader_WebUI.ViewModels
{
    public class AddSourceViewModel
    {
        [Required]
        [Display(Name = "Source name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "RSS channel link")]
        [Url(ErrorMessage = "Please enter a valid link")]
        public string Link { get; set; }
    }
}