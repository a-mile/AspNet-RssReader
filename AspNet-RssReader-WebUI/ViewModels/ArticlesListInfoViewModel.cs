namespace AspNet_RssReader_WebUI.ViewModels
{
    public class ArticlesListInfoViewModel
    {
        public string SourceName { get; set; }
        public string CategoryName { get; set; }
        public string SortOrder { get; set; } = "date_desc";
        public int Page { get; set; } = 1;
    }
}