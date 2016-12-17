using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AspNet_RssReader_Domain.Entities;
using AspNet_RssReader_WebUI.ViewModels;
using Microsoft.AspNet.Identity;

namespace AspNet_RssReader_WebUI.Controllers
{
    public class CategoryController : Controller
    {
        private readonly DbContext _dbContext;
        private ApplicationUser _currentUser;

        public CategoryController(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private ApplicationUser GetCurrentUser()
        {
            if (_currentUser == null)
            {
                _currentUser = _dbContext.Set<ApplicationUser>().Find(User.Identity.GetUserId());
            }

            return _currentUser;
        }

        public ActionResult AddNewCategory()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddNewCategory(AddCategoryViewModel addCategory)
        {
            if (GetCurrentUser().Sources.Any(x => x.Name == addCategory.Name))
            {
                ModelState.AddModelError("Name", "There is already source with this name");
            }

            if (!ModelState.IsValid)
            {
                return View(addCategory);
            }

            Category category = new Category()
            {
                Name = addCategory.Name,
                ApplicationUserId = GetCurrentUser().Id
            };

            _dbContext.Set<Category>().Add(category);
            _dbContext.SaveChanges();

            return RedirectToAction("List", "Article");
        }
    }
}