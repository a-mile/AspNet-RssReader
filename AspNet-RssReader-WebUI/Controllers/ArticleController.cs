using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AspNet_RssReader_Domain.Entities;
using AspNet_RssReader_WebUI.Extensions;
using AspNet_RssReader_WebUI.Synchronization;
using AspNet_RssReader_WebUI.ViewModels;

namespace AspNet_RssReader_WebUI.Controllers
{
    public class ArticleController : BaseController
    {                    
        public ActionResult List(ArticlesListInfoViewModel articlesListInfoViewModel)
        {
            var viewModel =
                ViewModelFactory.GetViewModel<ArticleController, ArticlesListInfoModelViewModel, ArticlesListInfoViewModel>(
                    this, articlesListInfoViewModel);

            if (viewModel == null)
                return HttpNotFound();

            ArticleSynchronizer synchronizer = new ArticleSynchronizer();

            if (viewModel.Category != null)
            {
                synchronizer.SynchronizeUserCategory(viewModel.Category);
            }
            else if (viewModel.Source != null)
            {
                synchronizer.SynchronizeUserSource(viewModel.Source);
            }
            else
            {
                synchronizer.SynchronizeAllUserSources();
            }

            Session["ArticlesListInfoViewModel"] = articlesListInfoViewModel;

            return View();
        } 

        public ActionResult Articles(int page)
        {
            var articlesListInfoViewModel = Session["ArticlesListInfoViewModel"] as ArticlesListInfoViewModel;
            articlesListInfoViewModel.Page = page;

            var viewModel =
                ViewModelFactory.GetViewModel<ArticleController, ArticlesListViewModel, ArticlesListInfoViewModel>(
                    this, articlesListInfoViewModel);

            if (viewModel == null)
                return HttpNotFound();

            return PartialView("ArticleSummary",viewModel);                        
        }

        public ActionResult MarkAllAsRead(string sourceName = null, string categoryName = null)
        {
            var currentUser = System.Web.HttpContext.Current.GetCurrentUser();
            var userArticles = currentUser.Articles;

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
               
            }

    
        }

        public string MarkArticleAsRead(int articleId)
        {
            var currentUser = System.Web.HttpContext.Current.GetCurrentUser();
            Article article = currentUser.Articles.FirstOrDefault(x => x.Id == articleId);

            if (article != null)
            {
                article.Read = true;


                return "Succes";
            }
           
            return "Failed";
        }
    }
}