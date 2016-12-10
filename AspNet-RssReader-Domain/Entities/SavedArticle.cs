using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNet_RssReader_Domain.Entities
{
    public class SavedArticle
    {
        [Column(Order = 0), Key, ForeignKey("Article")]
        public int ArticleId { get; set; }

        [Column(Order = 1), Key, ForeignKey("ApplicationUser")]
        public string ApplicationUserId { get; set; }

        public virtual Article Article { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
