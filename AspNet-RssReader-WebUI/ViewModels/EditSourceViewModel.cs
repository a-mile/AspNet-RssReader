using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AspNet_RssReader_WebUI.ViewModels
{
    public class EditSourceViewModel
    {
        public int SourceId { get; set; }

        [Required]
        [Display(Name = "Source name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "RSS channel link")]
        [Url(ErrorMessage = "Please enter a valid link")]
        public string Link { get; set; }

        [Display(Name = "Category")]
        public int? SelectedCategoryId { get; set; }

        public IEnumerable<CategoryViewModel> CategoriesViewModel { get; set; }

        public IEnumerable<SelectListItem> Categories => new SelectList(CategoriesViewModel, "Id", "Name");
    }
}