namespace AspNet_RssReader_WebUI.ViewModels
{
    public class ArticleViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public string SourceName { get; set; }
        public string PublicationTime { get; set; }
        public bool Read { get; set; }
        public string ImageUrl { get; set; }
    }
}