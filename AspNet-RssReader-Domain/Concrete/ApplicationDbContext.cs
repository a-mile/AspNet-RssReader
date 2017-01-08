using System.Data.Entity;
using AspNet_RssReader_Domain.Entities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace AspNet_RssReader_Domain.Concrete
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Article> Articles { get; set; }
        public DbSet<Source> Sources { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<SavedArticle> SavedArticles { get; set; }

        public ApplicationDbContext() : base("EFDbContext")
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}
