using System.Linq;
using System.Web.Mvc;
using AspNet_RssReader_Domain.Concrete;
using AspNet_RssReader_Domain.Entities;
using AspNet_RssReader_WebUI.Infrastructure;
using AspNet_RssReader_WebUI.Interfaces;
using AspNet_RssReader_WebUI.ViewModels;

namespace AspNet_RssReader_WebUI.FormHandlers
{
    public class CreateCategoryFormHandler : IGeneralFormHandler<CreateCategoryViewModel>
    {
        public void ProcessForm(ControllerContext context, CreateCategoryViewModel model)
        {
            var dbContext = context.GetDbContext<ApplicationDbContext>();
            var currentUser = context.GetCurrentUser<ApplicationUser>();

            Category category = new Category
            {
                Name = model.Name,
                ApplicationUserId = currentUser.Id
            };

            dbContext.Categories.Add(category);
            dbContext.SaveChanges();
        }
    }
    public class UpdateCategoryFormHandler : IGeneralFormHandler<UpdateCategoryViewModel>
    {
        public void ProcessForm(ControllerContext context, UpdateCategoryViewModel model)
        {
            var dbContext = context.GetDbContext<ApplicationDbContext>();
            var currentUser = context.GetCurrentUser<ApplicationUser>();
            var category = currentUser.Categories.FirstOrDefault(x => x.Id == model.Id);

            if (category != null)
            {
                category.Name = model.Name;
                dbContext.SaveChanges();
            }          
        }
    }
    public class DeleteCategoryFormHandler : IGeneralFormHandler<DeleteCategoryViewModel>
    {
        public void ProcessForm(ControllerContext context, DeleteCategoryViewModel model)
        {
            var dbContext = context.GetDbContext<ApplicationDbContext>();
            var currentUser = context.GetCurrentUser<ApplicationUser>();
            var category = currentUser.Categories.FirstOrDefault(x => x.Id == model.Id);

            if (category != null)
            {
                dbContext.Categories.Remove(category);
                dbContext.SaveChanges();
            }            
        }
    }

}