using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AspNet_RssReader_Domain.Abstract;
using AspNet_RssReader_Domain.Entities;
using AspNet_RssReader_WebUI.ViewModels;

namespace AspNet_RssReader_WebUI.Controllers
{
    public class ArticleController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IArticleDownloader _articleDownloader;
        private readonly IArticleExtractor _articleExtractor;
        private readonly int _pageSize = 9;

        public ArticleController(IUnitOfWork unitOfWork, IArticleDownloader articleDownloader, IArticleExtractor articleExtractor)
        {
            _unitOfWork = unitOfWork;
            _articleDownloader = articleDownloader;
            _articleExtractor = articleExtractor;
        }

        public ViewResult List(int? sourceId, string sortingBy = "PubDate", string sortingOrder = "Desc")
        {
            if (sourceId == null)
            {
                foreach (var source in _unitOfWork.Repository<Source>().SelectAll().ToList())
                {
                    SynchronizeArticles(source);
                }
            }
            else
            {
                Source source = _unitOfWork.Repository<Source>().GetById(sourceId.Value);
                SynchronizeArticles(source);
            }

            _unitOfWork.Save();

            ArticlesListViewModel articlesListViewModel = new ArticlesListViewModel
            {
                SourceId = sourceId,
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
                        source.Articles.Add(newArticle);
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
                    _unitOfWork.Repository<Article>().Delete(article);
                }

                _unitOfWork.Repository<Source>().Update(source);
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

        public JsonResult GetArticles(int? sourceId, string sortingBy, string sortingOrder, int page = 1)
        {
            IEnumerable<Article> articles;

            if (sortingOrder == "Asc")
            {
                articles = _unitOfWork.Repository<Article>()
                    .SelectAll(x => sourceId == null || x.Source.Id == sourceId)
                    .OrderBy(x => x.Read)
                    .ThenBy(x => GetArticlePropertyByName(x, sortingBy))
                    .Skip((page - 1)*_pageSize)
                    .Take(_pageSize);
            }
            else
            {
                articles = _unitOfWork.Repository<Article>()
                    .SelectAll(x => sourceId == null || x.Source.Id == sourceId)
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

        public ActionResult MarkAllAsRead(int? sourceId)
        {
            if (sourceId == null)
            {
                foreach (var source in _unitOfWork.Repository<Source>().SelectAll().ToList())
                {
                    MarkArticlesAsRead(source.Articles);
                }
            }
            else
            {
                Source source = _unitOfWork.Repository<Source>().GetById(sourceId.Value);
                MarkArticlesAsRead(source.Articles);
            }
            
            _unitOfWork.Save();

            return RedirectToAction("List", new {sourceId = sourceId, page = 1});
        }

        private void MarkArticlesAsRead(IEnumerable<Article> articles)
        {
            foreach (var article in articles)
            {
                article.Read = true;
                _unitOfWork.Repository<Article>().Update(article);
            }
        }

        public string MarkArticleAsRead(int articleId)
        {
            Article article = _unitOfWork.Repository<Article>().GetById(articleId);

            if (article != null)
            {
                article.Read = true;
            }

            _unitOfWork.Repository<Article>().Update(article);
            _unitOfWork.Save();

            return "Success";
        }
    }
}