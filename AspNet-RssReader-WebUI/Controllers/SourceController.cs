using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using AspNet_RssReader_Domain.Entities;
using AspNet_RssReader_WebUI.ViewModels;
using Microsoft.AspNet.Identity;

namespace AspNet_RssReader_WebUI.Controllers
{
    public class SourceController : Controller
    {
        private readonly DbContext _dbContext;

        public SourceController(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ViewResult AddNewSource()
        {
            return View(new AddSourceViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddNewSource(AddSourceViewModel addSource)
        {
            if (!ModelState.IsValid)
            {
                return View(addSource);
            }

            Source source = new Source
            {
                Name = addSource.Name,
                Link = addSource.Link,
                ApplicationUserId = User.Identity.GetUserId()
            };

            _dbContext.Set<Source>().Add(source);
            _dbContext.SaveChanges();

            return RedirectToAction("List", "Article");
        }

        public ActionResult DeleteSource(string sourceName)
        {
            string userId = User.Identity.GetUserId();

            Source source =
                _dbContext
                    .Set<Source>()
                    .FirstOrDefault(x => x.Name == sourceName && x.ApplicationUserId == userId);
                

            if (source == null)
                return View("Error");

            DeleteSourceViewModel deleteSourceViewModel = new DeleteSourceViewModel {Name = sourceName};

            return View(deleteSourceViewModel);
        }

        [HttpPost,ActionName("DeleteSource")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteSourceConfirmed(DeleteSourceViewModel deleteSource)
        {
            string userId = User.Identity.GetUserId();
            Source source =
                _dbContext
                    .Set<Source>()
                    .FirstOrDefault(x => x.Name == deleteSource.Name && x.ApplicationUserId == userId);

            _dbContext.Set<Source>().Remove(source);
            _dbContext.SaveChanges();

            return RedirectToAction("List", "Article");
        }
    }
}