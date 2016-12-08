using System.ComponentModel.DataAnnotations;

namespace AspNet_RssReader_WebUI.ViewModels
{
    public class DeleteSourceViewModel
    {
        public int SourceId { get; set; }
        public string Name { get; set; }
    }
}