using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;

namespace AspNet_RssReader_Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            Sources = new HashSet<Source>();
            Categories = new HashSet<Category>();
            Articles = new HashSet<Article>();
            SavedArticles = new HashSet<SavedArticle>();
        }
        public virtual ICollection<Source> Sources { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
        public virtual ICollection<Article> Articles { get; set; }
        public virtual ICollection<SavedArticle> SavedArticles { get; set; }
    }
}
