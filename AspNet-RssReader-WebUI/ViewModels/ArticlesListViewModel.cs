using System.Collections.Generic;

namespace AspNet_RssReader_WebUI.ViewModels
{
    public class ArticlesListViewModel
    {
        public IEnumerable<ArticleViewModel> ArticleViewModels { get; set; }
    }
}