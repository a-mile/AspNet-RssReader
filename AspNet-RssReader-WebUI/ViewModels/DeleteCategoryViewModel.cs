using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AspNet_RssReader_WebUI.ViewModels
{
    public class DeleteCategoryViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public string Name { get; set; }
    }
}