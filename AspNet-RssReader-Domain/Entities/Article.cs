using System;
using System.ComponentModel.DataAnnotations;

namespace AspNet_RssReader_Domain.Entities
{
    public class Article
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Link { get; set; }
        public string Description { get; set; }
        public bool Read { get; set; }
        public DateTime PubDate { get; set; }
        public string ImageUrl { get; set; }
        public int SourceId { get; set; }
        public string Author { get; set; }
        public virtual Source Source { get; set; }
    }
}
