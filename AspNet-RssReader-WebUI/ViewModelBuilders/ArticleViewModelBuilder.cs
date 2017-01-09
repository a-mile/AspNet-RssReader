using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AspNet_RssReader_Domain.Entities;
using AspNet_RssReader_WebUI.Controllers;
using AspNet_RssReader_WebUI.Extensions;
using AspNet_RssReader_WebUI.Interfaces;
using AspNet_RssReader_WebUI.ViewModels;

namespace AspNet_RssReader_WebUI.ViewModelBuilders
{
    public class ArticleListInfoViewModelBuilder : IViewModelBuilder<ArticleController,ArticlesListInfoModelViewModel,ArticlesListInfoViewModel>
    {
        public ArticlesListInfoModelViewModel Build(ArticleController controller, ArticlesListInfoModelViewModel viewModel,
            ArticlesListInfoViewModel input)
        {
            var currentUser = HttpContext.Current.GetCurrentUser();

            if (input.CategoryName != null)
            {
                var category = currentUser.Categories.FirstOrDefault(x => x.Name == input.CategoryName);

                if (category == null)
                    return null;

                viewModel.Category = category;
            }
            if (input.SourceName != null)
            {
                if (input.CategoryName != null)
                    return null;

                var source = currentUser.Sources.FirstOrDefault(x => x.Name == input.SourceName);

                if (source == null)
                    return null;

                viewModel.Source = source;
            }

            return viewModel;
        }             
    }

    public class ArticleListViewModelBuilder :
        IViewModelBuilder<ArticleController, ArticlesListViewModel, ArticlesListInfoViewModel>
    {
        public ArticlesListViewModel Build(ArticleController controller, ArticlesListViewModel viewModel,
            ArticlesListInfoViewModel input)
        {
            var articles = Enumerable.Empty<Article>();
            var currentUser = HttpContext.Current.GetCurrentUser();

            int pageSize = 9; 

            if (input.CategoryName == null)
            {
                switch (input.SortOrder)
                {
                    case "date_asc":
                        articles = currentUser.Articles
                            .Where(x => input.SourceName == null || x.Source.Name == input.SourceName)
                            .OrderBy(x => x.Read)
                            .ThenBy(x => x.PubDate)
                            .Skip((input.Page - 1) * pageSize)
                            .Take(pageSize);
                        break;
                    case "date_desc":
                        articles = currentUser.Articles
                            .Where(x => input.SourceName == null || x.Source.Name == input.SourceName)
                            .OrderBy(x => x.Read)
                            .ThenByDescending(x => x.PubDate)
                            .Skip((input.Page - 1) * pageSize)
                            .Take(pageSize);
                        break;
                }
            }
            else
            {
                switch (input.SortOrder)
                {
                    case "date_asc":
                        articles = currentUser.Articles
                            .Where(x => x.Source.CategoryId != null && x.Source.Category.Name == input.CategoryName)
                            .OrderBy(x => x.Read)
                            .ThenBy(x => x.PubDate)
                            .Skip((input.Page - 1) * pageSize)
                            .Take(pageSize);
                        break;
                    case "date_desc":
                        articles = currentUser.Articles
                            .Where(x => x.Source.CategoryId != null && x.Source.Category.Name == input.CategoryName)
                            .OrderBy(x => x.Read)
                            .ThenByDescending(x => x.PubDate)
                            .Skip((input.Page - 1) * pageSize)
                            .Take(pageSize);
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

            viewModel.ArticleViewModels = articleViewModels;

            return viewModel;
        }
        private string GetPublicationTime(DateTime date)
        {
            TimeSpan difference = DateTime.Now - date;

            int days = (int)difference.TotalDays;
            if (days > 1)
                return days + " days ago";
            if (days == 1)
                return days + " day ago";

            int hours = (int)difference.TotalHours;
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
    }

}