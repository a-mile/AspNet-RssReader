using System.Collections.Generic;
using AspNet_RssReader_Domain.Entities;

namespace AspNet_RssReader_WebUI.ViewModels
{
    public class NavViewModel
    {
        public IEnumerable<Source> Sources { get; set; }
        public IEnumerable<Category> Categories { get; set; }
    }
}