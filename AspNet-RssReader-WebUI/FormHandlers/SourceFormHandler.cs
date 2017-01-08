using System.Linq;
using System.Web.Mvc;
using AspNet_RssReader_Domain.Concrete;
using AspNet_RssReader_Domain.Entities;
using AspNet_RssReader_WebUI.Infrastructure;
using AspNet_RssReader_WebUI.Interfaces;
using AspNet_RssReader_WebUI.ViewModels;

namespace AspNet_RssReader_WebUI.FormHandlers
{
    public class CreateSourceFormHandler : IGeneralFormHandler<CreateSourceViewModel>
    {
        public void ProcessForm(ControllerContext context, CreateSourceViewModel model)
        {
            var dbContext = context.GetDbContext<ApplicationDbContext>();
            var currentUser = context.GetCurrentUser<ApplicationUser>();

            Source source = new Source()
            {
                Name = model.Name,
                Link = model.Link,
                CategoryId = model.CategoryId,
                ApplicationUserId = currentUser.Id
            };

            dbContext.Sources.Add(source);
            dbContext.SaveChanges();
        }
    }

    public class UpdateSourceFormHandler : IGeneralFormHandler<UpdateSourceViewModel>
    {
        public void ProcessForm(ControllerContext context, UpdateSourceViewModel model)
        {
            var dbContext = context.GetDbContext<ApplicationDbContext>();
            var currentUser = context.GetCurrentUser<ApplicationUser>();
            var source = currentUser.Sources.FirstOrDefault(x => x.Id == model.Id);

            if (source != null)
            {
                source.Name = model.Name;
                source.Link = model.Link;
                dbContext.SaveChanges();
            }
        }
    }

    public class DeleteSourceFormHandler : IGeneralFormHandler<DeleteSourceViewModel>
    {
        public void ProcessForm(ControllerContext context, DeleteSourceViewModel model)
        {
            var dbContext = context.GetDbContext<ApplicationDbContext>();
            var currentUser = context.GetCurrentUser<ApplicationUser>();
            var source = currentUser.Sources.FirstOrDefault(x => x.Id == model.Id);

            if (source != null)
            {
                dbContext.Sources.Remove(source);
                dbContext.SaveChanges();
            }
        }
    }
}