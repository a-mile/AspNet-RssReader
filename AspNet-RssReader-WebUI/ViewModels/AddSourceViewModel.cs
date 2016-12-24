using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using AspNet_RssReader_Domain.Entities;
using AspNet_RssReader_WebUI.Validation;

namespace AspNet_RssReader_WebUI.ViewModels
{
    public class AddSourceViewModel
    {      
        [Required]
        [Display(Name = "Source name")]
        [StringLength(15)]
        public string Name { get; set; }

        [Required]
        [Display(Name = "RSS channel link")]
        [RssLinkValidator]
        [StringLength(200)]
        public string Link { get; set; }

        [Display(Name = "Category")]
        public int? SelectedCategoryId { get; set; }

        public IEnumerable<CategoryViewModel> CategoriesViewModel { get; set; }

        public IEnumerable<SelectListItem> Categories => new SelectList(CategoriesViewModel,"Id","Name");
    }
}