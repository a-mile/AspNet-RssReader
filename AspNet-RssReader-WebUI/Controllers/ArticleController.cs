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
    public class ArticleController : Controller
    {
        private readonly DbContext _dbContext;
        private readonly IArticleDownloader _articleDownloader;
        private readonly IArticleExtractor _articleExtractor;      
        private readonly int _pageSize = 9;

        public ArticleController(DbContext dbContext, IArticleDownloader articleDownloader,
            IArticleExtractor articleExtractor)
        {
            _dbContext = dbContext;
            _articleDownloader = articleDownloader;
            _articleExtractor = articleExtractor;            
        }

        public ViewResult List(string sourceName = null, string sortingBy = "PubDate", string sortingOrder = "Desc")
        {
            string userId = User.Identity.GetUserId();

            if (sourceName == null)
            {                
                var userSources =
                    _dbContext.Set<Source>().Where(x => x.ApplicationUserId == userId).ToList();

                foreach (var source in userSources)
                {
                    SynchronizeArticles(source);
                }
            }
            else
            {
                Source source =
                    _dbContext
                        .Set<Source>()
                        .FirstOrDefault(x => x.ApplicationUserId == userId && x.Name == sourceName);

                if (source == null)
                    return View("Error");

                SynchronizeArticles(source);
            }

            _dbContext.SaveChanges();

            ArticlesListViewModel articlesListViewModel = new ArticlesListViewModel
            {
                SourceName = sourceName,
                SortingBy = sortingBy,
                SortingOrder = sortingOrder
            };

            return View(articlesListViewModel);
        }

        private void SynchronizeArticles(Source source)
        {
            IEnumerable<Article> newArticles = _articleDownloader.GetArticles(source.Link).ToList();

            if (newArticles.Any())
            {            
                foreach (var newArticle in newArticles)
                {
                    if (source.Articles.Count(x => x.Link == newArticle.Link) == 0)
                    {
                        newArticle.ImageUrl = _articleExtractor.GetArticleLeadImageUrl(newArticle.Link);
                        newArticle.SourceId = source.Id;
                        newArticle.ApplicationUserId = User.Identity.GetUserId();

                        _dbContext.Set<Article>().Add(newArticle);
                    }
                }

                ICollection<Article> articlesToDelete = new List<Article>();

                foreach (var article in source.Articles)
                {
                    if (article.Read && newArticles.Count(x => x.Link == article.Link) == 0)
                        articlesToDelete.Add(article);
                }

                foreach (var article in articlesToDelete)
                {
                    _dbContext.Entry(article).State = EntityState.Deleted;
                }
            }
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

        public JsonResult GetArticles(string sourceName, string sortingBy, string sortingOrder, int page)
        {
            string userId = User.Identity.GetUserId();
            IEnumerable<Article> articles;

            if (sourceName == string.Empty)
                sourceName = null;

            if (sortingOrder == "Asc")
            {
                articles = _dbContext.Set<Article>()
                    .Where(x => (sourceName == null || x.Source.Name == sourceName) && x.ApplicationUserId == userId).ToList()
                    .OrderBy(x => x.Read)
                    .ThenBy(x => GetArticlePropertyByName(x, sortingBy))
                    .Skip((page - 1)*_pageSize)
                    .Take(_pageSize);
            }
            else
            {
                articles = _dbContext.Set<Article>()
                    .Where(x => (sourceName == null || x.Source.Name == sourceName) && x.ApplicationUserId == userId).ToList()
                    .OrderBy(x => x.Read)
                    .ThenByDescending(x => GetArticlePropertyByName(x, sortingBy))
                    .Skip((page - 1)*_pageSize)
                    .Take(_pageSize);
            }

            List<ArticleViewModel> articleViewModelList = new List<ArticleViewModel>();

            foreach (var article in articles)
            {
                ArticleViewModel articleViewModel = new ArticleViewModel
                {
                    Title = article.Title,
                    Description = article.Description,
                    Link = article.Link,
                    SourceName = article.Source.Name,
                    PublicationTime = GetPublicationTime(article.PubDate),
                    Read = article.Read ? "read" : "",
                    ImageUrl = article.ImageUrl,
                    Id = article.Id
                };

                articleViewModelList.Add(articleViewModel);
            }

            return Json(articleViewModelList, JsonRequestBehavior.AllowGet);
        }

        private object GetArticlePropertyByName(Article article, string propertyName)
        {
            return article.GetType().GetProperty(propertyName).GetValue(article, null);
        }

        public ActionResult MarkAllAsRead(string sourceName = null)
        {
            string userId = User.Identity.GetUserId();

            var userArticles = _dbContext.Set<Article>()
                    .Where(x => x.ApplicationUserId == userId);

            if (sourceName == null)
            {              
                MarkArticlesAsRead(userArticles);
            }
            else
            {
                var sourceArticles = userArticles.Where(x => x.Source.Name == sourceName);
                MarkArticlesAsRead(sourceArticles);
            }

            _dbContext.SaveChanges();

            return RedirectToAction("List", new {sourceName = sourceName});
        }

        private void MarkArticlesAsRead(IEnumerable<Article> articles)
        {
            foreach (var article in articles)
            {
                article.Read = true;
                _dbContext.Entry(article).State = EntityState.Modified;
            }
        }

        public string MarkArticleAsRead(int articleId)
        {
            Article article = _dbContext.Set<Article>().Find(articleId);

            if (article != null)
            {
                article.Read = true;
            }

            _dbContext.Entry(article).State = EntityState.Modified;
            _dbContext.SaveChanges();

            return "Success";
        }
    }
}