using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using AspNet_RssReader_Domain.Entities;
using AspNet_RssReader_WebUI.ViewModels;
using Microsoft.AspNet.Identity;

namespace AspNet_RssReader_WebUI.Controllers
{
    public class SourceController : BaseController
    {
        public SourceController(DbContext dbContext) : base(dbContext){ }   

        public ViewResult AddNewSource()
        {
            IEnumerable<Category> userCategories = CurrentUser.Categories;
            List<CategoryViewModel> userCategoriesViewModel = new List<CategoryViewModel>
            {
                new CategoryViewModel()
                {
                    Name = "None",
                    Id = null
                }
            };

            foreach (var category in userCategories)
            {
                CategoryViewModel categoryViewModel = new CategoryViewModel()
                {
                    Name = category.Name,
                    Id = category.Id
                };
                userCategoriesViewModel.Add(categoryViewModel);
            }

            return View(new AddSourceViewModel {CategoriesViewModel =  userCategoriesViewModel});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddNewSource(AddSourceViewModel addSource)
        {
            if (CurrentUser.Sources.Any(x =>x.Name == addSource.Name))
            {
                ModelState.AddModelError("Name", "There is already source with this name");
            }

            if (CurrentUser.Sources.Any(x => x.Link == addSource.Link))
            {
                ModelState.AddModelError("Link", "There is already source with this link");
            }

            if (!ModelState.IsValid)
            {
                IEnumerable<Category> userCategories = CurrentUser.Categories;
                List<CategoryViewModel> userCategoriesViewModel = new List<CategoryViewModel>
                {
                    new CategoryViewModel()
                    {
                        Name = "None",
                        Id = null
                    }
                };

                foreach (var category in userCategories)
                {
                    CategoryViewModel categoryViewModel = new CategoryViewModel()
                    {
                        Name = category.Name,
                        Id = category.Id
                    };
                    userCategoriesViewModel.Add(categoryViewModel);
                }
                addSource.CategoriesViewModel = userCategoriesViewModel;

                return View(addSource);
            }

            Source source = new Source
            {
                Name = addSource.Name,
                Link = addSource.Link,
                ApplicationUserId = CurrentUser.Id,
                CategoryId = addSource.SelectedCategoryId
            };  

            DbContext.Set<Source>().Add(source);
            DbContext.SaveChanges();

            return RedirectToAction("List", "Article");
        }

        public ViewResult EditSource(string sourceName)
        {
            Source source = CurrentUser.Sources.FirstOrDefault(x => x.Name == sourceName);

            if (source == null)
                return View("Error");

            IEnumerable<Category> userCategories = CurrentUser.Categories;
            List<CategoryViewModel> userCategoriesViewModel = new List<CategoryViewModel>
            {
                new CategoryViewModel()
                {
                    Name = "None",
                    Id = null
                }
            };

            foreach (var category in userCategories)
            {
                CategoryViewModel categoryViewModel = new CategoryViewModel()
                {
                    Name = category.Name,
                    Id = category.Id
                };
                userCategoriesViewModel.Add(categoryViewModel);
            }

            EditSourceViewModel editSourceViewModel = new EditSourceViewModel
            {
                CategoriesViewModel = userCategoriesViewModel,
                Link = source.Link,
                Name = source.Name,
                SelectedCategoryId = source.CategoryId,
                SourceId =  source.Id
            };
            return View(editSourceViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditSource(EditSourceViewModel editSource)
        {
            Source source = CurrentUser.Sources.FirstOrDefault(x=>x.Id == editSource.SourceId);

            if (source == null)
                return View("Error");

            if (editSource.Name != source.Name)
            {
                if (CurrentUser.Sources.Any(x => x.Name == editSource.Name))
                {
                    ModelState.AddModelError("Name", "There is already source with this name");
                }
            }

            if (editSource.Link != source.Link)
            {
                if (CurrentUser.Sources.Any(x => x.Link == editSource.Link))
                {
                    ModelState.AddModelError("Link", "There is already source with this link");
                }              
            }

            if (!ModelState.IsValid)
            {
                IEnumerable<Category> userCategories = CurrentUser.Categories;
                List<CategoryViewModel> userCategoriesViewModel = new List<CategoryViewModel>
                {
                    new CategoryViewModel()
                    {
                        Name = "None",
                        Id = null
                    }
                };

                foreach (var category in userCategories)
                {
                    CategoryViewModel categoryViewModel = new CategoryViewModel()
                    {
                        Name = category.Name,
                        Id = category.Id
                    };
                    userCategoriesViewModel.Add(categoryViewModel);
                }
                editSource.CategoriesViewModel = userCategoriesViewModel;

                return View(editSource);
            }

            source.Name = editSource.Name;
            source.Link = editSource.Link;
            source.CategoryId = editSource.SelectedCategoryId;

            DbContext.Entry(source).State = EntityState.Modified;
            DbContext.SaveChanges();

            return RedirectToAction("List", "Article");
        }

        public ActionResult DeleteSource(string sourceName)
        {
            Source source =
               CurrentUser.Sources
                    .FirstOrDefault(x => x.Name == sourceName);
                

            if (source == null)
                return View("Error");

            DeleteSourceViewModel deleteSourceViewModel = new DeleteSourceViewModel {Name = sourceName};

            return View(deleteSourceViewModel);
        }

        [HttpPost,ActionName("DeleteSource")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteSourceConfirmed(DeleteSourceViewModel deleteSource, string sure)
        {
            if (sure == "Yes")
            {
                Source source =
                    CurrentUser.Sources
                        .FirstOrDefault(x => x.Name == deleteSource.Name);

                DbContext.Set<Source>().Remove(source);
                DbContext.SaveChanges();
            }

            return RedirectToAction("List", "Article");
        }
    }
}