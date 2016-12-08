using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AspNet_RssReader_Domain.Entities
{
    public class Source
    {
        public Source()
        {
            Articles = new HashSet<Article>();
        }
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public virtual ICollection<Article> Articles { get; set; }
    }
}
