using System.Linq;
using System.Web;
using System.Web.Mvc;
using AspNet_RssReader_Domain.Entities;
using AspNet_RssReader_WebUI.Extensions;
using AspNet_RssReader_WebUI.Interfaces;
using AspNet_RssReader_WebUI.ViewModels;

namespace AspNet_RssReader_WebUI.FormHandlers
{
    public class CreateSourceFormHandler : IGeneralFormHandler<CreateSourceViewModel>
    {
        public void ProcessForm(ControllerContext context, CreateSourceViewModel model)
        {
            var dbContext = HttpContext.Current.GetDbContext();
            var currentUser = HttpContext.Current.GetCurrentUser();;

            Source source = new Source()
            {
                Name = model.Name,
                Link = model.Link,
                CategoryId = model.CategoryId,
            };

            currentUser.Sources.Add(source);
            dbContext.SaveChanges();
        }
    }

    public class UpdateSourceFormHandler : IGeneralFormHandler<UpdateSourceViewModel>
    {
        public void ProcessForm(ControllerContext context, UpdateSourceViewModel model)
        {
            var dbContext = HttpContext.Current.GetDbContext();
            var currentUser = HttpContext.Current.GetCurrentUser();;
            var source = currentUser.Sources.FirstOrDefault(x => x.Id == model.Id);

            if (source != null)
            {
                source.Name = model.Name;
                source.Link = model.Link;
                source.CategoryId = model.CategoryId;
                dbContext.SaveChanges();
            }
        }
    }

    public class DeleteSourceFormHandler : IGeneralFormHandler<DeleteSourceViewModel>
    {
        public void ProcessForm(ControllerContext context, DeleteSourceViewModel model)
        {
            var dbContext = HttpContext.Current.GetDbContext();
            var currentUser = HttpContext.Current.GetCurrentUser();;
            var source = currentUser.Sources.FirstOrDefault(x => x.Id == model.Id);

            if (source != null)
            {
                currentUser.Sources.Remove(source);
                dbContext.SaveChanges();
            }
        }
    }
}