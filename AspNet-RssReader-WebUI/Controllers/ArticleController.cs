using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using AspNet_RssReader_Domain.Abstract;
using AspNet_RssReader_Domain.Entities;
using AspNet_RssReader_WebUI.ViewModels;
using Microsoft.AspNet.Identity;

namespace AspNet_RssReader_WebUI.Controllers
{
    public class ArticleController : BaseController
    {
        private readonly IArticleDownloader _articleDownloader;
        private readonly int _pageSize = 9;
        private readonly TimeSpan _deleteReadArticlesOlderThan = new TimeSpan(7, 0, 0, 0);

        public ArticleController(DbContext dbContext, IArticleDownloader articleDownloader) : base(dbContext)
        {
            _articleDownloader = articleDownloader;
        }        

        public ViewResult List(string sourceName = null, string categoryName = null, string sortOrder = "date_desc")
        {
            ArticlesListInfoViewModel articlesListInfoViewModel = null;

            if (CurrentUser.Sources.Any())
            {
                if (sourceName == null)
                {
                    var userSources = CurrentUser.Sources.ToList();

                    if (categoryName != null)
                    {
                        userSources = userSources.Where(x => x.CategoryId != null && x.Category.Name == categoryName).ToList();
                    }                    
                   
                    foreach (var source in userSources)
                    {
                        SynchronizeArticles(source);
                    }
                }
                else
                {
                    Source source = CurrentUser.Sources.FirstOrDefault(x => x.Name == sourceName);

                    if (source == null)
                        return View("Error");

                    SynchronizeArticles(source);
                }

                articlesListInfoViewModel = new ArticlesListInfoViewModel
                {
                    SourceName = sourceName,
                    CategoryName = categoryName,
                    SortOrder = sortOrder
                };
            }
           
            return View(articlesListInfoViewModel);
        }

        private void SynchronizeArticles(Source source)
        {
            var newArticles = _articleDownloader.GetArticles(source.Link).ToList();

            if (newArticles.Any())
            {
                if (source.SyncDate == null)
                {
                    foreach (var newArticle in newArticles)
                    {                       
                        newArticle.SourceId = source.Id;
                        newArticle.ApplicationUserId = CurrentUser.Id;

                        DbContext.Set<Article>().Add(newArticle);                       
                    }                  
                }
                else
                {
                    var articlesNewerThanSyncDate = newArticles.Where(x => x.PubDate > source.SyncDate);

                    foreach (var newArticle in articlesNewerThanSyncDate)
                    {
                        newArticle.SourceId = source.Id;
                        newArticle.ApplicationUserId = CurrentUser.Id;

                        DbContext.Set<Article>().Add(newArticle);
                    }
                }

                source.SyncDate = DateTime.Now;
                DbContext.SaveChanges();
            }

            DeleteOldArticles(source);
        }

        private void DeleteOldArticles(Source source)
        {
            var articlesToDelete =
                source.Articles.Where(x => DateTime.Now - x.PubDate > _deleteReadArticlesOlderThan).ToList();

            foreach (var article in articlesToDelete)
            {
                DbContext.Entry(article).State = EntityState.Deleted;
            }

            DbContext.SaveChanges();
        }

        private string GetPublicationTime(DateTime date)
        {
            TimeSpan difference = DateTime.Now - date;

            int days =  (int)difference.TotalDays;
            if (days > 1)
                return days + " days ago";
            if (days == 1)
                return days + " day ago";

            int hours = (int) difference.TotalHours;
            if (hours > 1)
                return hours + " hours ago";
            if (hours == 1)
                return hours + " hour ago";

            int minutes = (int)difference.TotalMinutes;
            if (minutes > 1)
                return minutes + " minutes ago";
            if (minutes == 1)
                return minutes + " minute ago";

            return "Just now";
        }

        public ActionResult GetArticles(string sourceName, string categoryName, string sortOrder, int page)
        {
            var articles = Enumerable.Empty<Article>();

            if (sourceName == string.Empty)
                sourceName = null;
            if (categoryName == string.Empty)
                categoryName = null;

            if (categoryName == null)
            {
                switch (sortOrder)
                {
                    case "date_asc":
                        articles = CurrentUser.Articles
                            .Where(x => sourceName == null || x.Source.Name == sourceName)
                            .OrderBy(x => x.Read)
                            .ThenBy(x => x.PubDate)
                            .Skip((page - 1)*_pageSize)
                            .Take(_pageSize);
                        break;
                    case "date_desc":
                        articles = CurrentUser.Articles
                            .Where(x => sourceName == null || x.Source.Name == sourceName)
                            .OrderBy(x => x.Read)
                            .ThenByDescending(x => x.PubDate)
                            .Skip((page - 1)*_pageSize)
                            .Take(_pageSize);
                        break;
                }
            }
            else
            {
                switch (sortOrder)
                {
                    case "date_asc":
                        articles = CurrentUser.Articles
                            .Where(x => x.Source.CategoryId != null && x.Source.Category.Name == categoryName)
                            .OrderBy(x => x.Read)
                            .ThenBy(x => x.PubDate)
                            .Skip((page - 1) * _pageSize)
                            .Take(_pageSize);
                        break;
                    case "date_desc":
                        articles = CurrentUser.Articles
                            .Where(x => x.Source.CategoryId != null && x.Source.Category.Name == categoryName)
                            .OrderBy(x => x.Read)
                            .ThenByDescending(x => x.PubDate)
                            .Skip((page - 1) * _pageSize)
                            .Take(_pageSize);
                        break;
                }
            }

            List<ArticleViewModel> articleViewModels = new List<ArticleViewModel>();

            foreach (var article in articles)
            {
                ArticleViewModel articleViewModel = new ArticleViewModel
                {
                    Title = article.Title,
                    Description = article.Description,
                    Link = article.Link,
                    SourceName = article.Source.Name,
                    PublicationTime = GetPublicationTime(article.PubDate),
                    Read = article.Read,
                    ImageUrl = article.ImageUrl,
                    Id = article.Id
                };

                articleViewModels.Add(articleViewModel);
            }

            ArticlesListViewModel articlesListViewModel = new ArticlesListViewModel
            {
                ArticleViewModels = articleViewModels
            };

            return PartialView("ArticleSummary", articlesListViewModel);
        }

        public ActionResult MarkAllAsRead(string sourceName = null, string categoryName = null)
        {
            var userArticles = CurrentUser.Articles;

            if (sourceName == null)
            {
                if (categoryName != null)
                {
                    userArticles = userArticles.Where(x => x.Source.CategoryId != null && x.Source.Category.Name == categoryName).ToList();
                }
                MarkArticlesAsRead(userArticles);
            }
            else
            {
                var sourceArticles = userArticles.Where(x => x.Source.Name == sourceName);
                MarkArticlesAsRead(sourceArticles);
            }
         
            return RedirectToAction("List", new {sourceName = sourceName, categoryName = categoryName});
        }

        private void MarkArticlesAsRead(IEnumerable<Article> articles)
        {
            foreach (var article in articles)
            {
                article.Read = true;
                DbContext.Entry(article).State = EntityState.Modified;
            }

            DbContext.SaveChanges();
        }

        public string MarkArticleAsRead(int articleId)
        {
            Article article = CurrentUser.Articles.FirstOrDefault(x => x.Id == articleId);

            if (article != null)
            {
                article.Read = true;
                DbContext.Entry(article).State = EntityState.Modified;
                DbContext.SaveChanges();

                return "Succes";
            }
           
            return "Failed";
        }
    }
}