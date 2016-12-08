using System.Collections.Generic;
using AspNet_RssReader_Domain.Entities;

namespace AspNet_RssReader_Domain.Abstract
{
    public interface IArticleDownloader
    {
        IEnumerable<Article> GetArticles(string sourceLink);
    }
}
