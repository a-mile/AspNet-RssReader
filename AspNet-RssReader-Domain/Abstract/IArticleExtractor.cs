namespace AspNet_RssReader_Domain.Abstract
{
    public interface IArticleExtractor
    {
        string GetArticleContent(string articleUrl);
        string GetArticleLeadImageUrl(string articleUrl);
    }
}
