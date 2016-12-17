using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        [ForeignKey("ApplicationUser")]
        public string ApplicationUserId { get; set; }

        [ForeignKey("Category")]
        public int? CategoryId { get; set; }

        public string Name { get; set; }
        public string Link { get; set; }
        public DateTime? SyncDate { get; set; }

        public virtual ICollection<Article> Articles { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual Category Category { get; set; }
    }
}
