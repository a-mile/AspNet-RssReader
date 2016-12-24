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
    public class CategoryController : BaseController
    {
        public CategoryController(DbContext dbContext) : base(dbContext){ }

        public ActionResult AddNewCategory()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddNewCategory(AddCategoryViewModel addCategory)
        {
            if (CurrentUser.Categories.Any(x => x.Name == addCategory.Name))
            {
                ModelState.AddModelError("Name", "There is already category with this name");
            }

            if (!ModelState.IsValid)
            {
                return View(addCategory);
            }

            Category category = new Category()
            {
                Name = addCategory.Name,
                ApplicationUserId = CurrentUser.Id
            };

            DbContext.Set<Category>().Add(category);
            DbContext.SaveChanges();

            return RedirectToAction("List", "Article");
        }

        public ViewResult EditCategory(string categoryName)
        {
            Category category = CurrentUser.Categories.FirstOrDefault(x => x.Name == categoryName);

            if (category == null)
                return View("Error");

            EditCategoryViewModel editCategoryViewModel = new EditCategoryViewModel
            {
                Name = category.Name,
                CategoryId = category.Id
            };

            return View(editCategoryViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCategory(EditCategoryViewModel editCategory)
        {
            Category category = CurrentUser.Categories.FirstOrDefault(x => x.Id == editCategory.CategoryId);

            if (category == null)
                return View("Error");

            if (editCategory.Name != category.Name)
            {
                if (CurrentUser.Categories.Any(x => x.Name == editCategory.Name))
                {
                    ModelState.AddModelError("Name", "There is already category with this name");
                }
            }            

            if (!ModelState.IsValid)
            {             
                return View(editCategory);
            }

            category.Name = editCategory.Name;

            DbContext.Entry(category).State = EntityState.Modified;
            DbContext.SaveChanges();

            return RedirectToAction("List", "Article");
        }

        public ActionResult DeleteCategory(string categoryName)
        {
            Category category =
               CurrentUser.Categories
                    .FirstOrDefault(x => x.Name == categoryName);


            if (category == null)
                return View("Error");

            DeleteCategoryViewModel deleteCategoryViewModel = new DeleteCategoryViewModel { Name = categoryName };

            return View(deleteCategoryViewModel);
        }

        [HttpPost, ActionName("DeleteCategory")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteCategoryConfirmed(DeleteCategoryViewModel deleteCategory, string sure)
        {
            if (sure == "Yes")
            {               
                Category category =
                    CurrentUser.Categories
                        .FirstOrDefault(x => x.Name == deleteCategory.Name);

                if (category == null)
                    return View("Error");

                category = DbContext.Set<Category>().Include("Sources").FirstOrDefault(x => x.Id == category.Id);

                DbContext.Set<Category>().Remove(category);
                DbContext.SaveChanges();
            }

            return RedirectToAction("List", "Article");
        }
    }
}