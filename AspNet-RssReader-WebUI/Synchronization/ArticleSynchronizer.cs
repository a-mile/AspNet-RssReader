using System;
using System.Data.Entity;
using System.Linq;
using System.Web;
using AspNet_RssReader_Domain.Concrete;
using AspNet_RssReader_Domain.Entities;
using Microsoft.AspNet.Identity;

namespace AspNet_RssReader_WebUI.Synchronization
{
    public class ArticleSynchronizer
    {
        private readonly ApplicationUser _currentUser;
        private readonly ApplicationDbContext _dbContext = new ApplicationDbContext();
        private readonly XmlArticleDownloader _articleDownloader = new XmlArticleDownloader();
        private readonly TimeSpan _deleteReadArticlesOlderThan = new TimeSpan(7, 0, 0, 0);

        public ArticleSynchronizer()
        {
            var currentUserId = HttpContext.Current.User.Identity.GetUserId();           
            _currentUser = _dbContext.Users.Find(currentUserId);          
        }

        public void SynchronizeAllUserSources()
        {
            foreach (var source in _currentUser.Sources)
            {
                SynchronizeUserSource(source);
            }
        }

        public void SynchronizeUserCategory(Category category)
        {
            foreach (var source in category.Sources)
            {
                SynchronizeUserSource(source);
            }
        }

        public void SynchronizeUserSource(Source source)
        {
            var newArticles = _articleDownloader.GetArticles(source.Link).ToList();

            if (newArticles.Any())
            {
                if (source.SyncDate == null)
                {
                    foreach (var newArticle in newArticles)
                    {
                        newArticle.SourceId = source.Id;
                        _currentUser.Articles.Add(newArticle);
                    }
                }
                else
                {
                    var articlesNewerThanSyncDate = newArticles.Where(x => x.PubDate > source.SyncDate);

                    foreach (var newArticle in articlesNewerThanSyncDate)
                    {
                        newArticle.SourceId = source.Id;
                        _currentUser.Articles.Add(newArticle);
                    }
                }

                source.SyncDate = DateTime.Now;
            }

            DeleteOldArticles(source);

            _dbContext.SaveChanges();
        }

        private void DeleteOldArticles(Source source)
        {
            var articlesToDelete =
                source.Articles.Where(x => DateTime.Now - x.PubDate > _deleteReadArticlesOlderThan).ToList();

            foreach (var article in articlesToDelete)
            {
                _dbContext.Entry(article).State = EntityState.Deleted;
            }
        }
    }
}