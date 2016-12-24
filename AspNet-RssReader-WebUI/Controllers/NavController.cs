using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using AspNet_RssReader_Domain.Abstract;
using AspNet_RssReader_Domain.Entities;
using AspNet_RssReader_WebUI.ViewModels;
using Microsoft.AspNet.Identity;

namespace AspNet_RssReader_WebUI.Controllers
{
    public class NavController : BaseController
    {
        public NavController(DbContext dbContext) : base(dbContext){ }
        public PartialViewResult Menu(string sourceName = null)
        {
            NavMenuViewModel sourcesList = new NavMenuViewModel
            {
                Sources = CurrentUser.Sources,
                CurrentSourceName = sourceName,
                Categories = CurrentUser.Categories
            };
            return PartialView(sourcesList);
        }
    }
}