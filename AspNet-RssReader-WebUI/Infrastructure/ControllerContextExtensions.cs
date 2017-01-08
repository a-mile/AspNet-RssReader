using System.Web.Mvc;
using AspNet_RssReader_Domain.Concrete;
using AspNet_RssReader_Domain.Entities;
using Microsoft.AspNet.Identity;
using Ninject;
using Ninject.Web.Common;

namespace AspNet_RssReader_WebUI.Infrastructure
{
    public static class ControllerContextExtensions
    {
        public static T GetDbContext<T>(this ControllerContext webContext) where T : ApplicationDbContext
        {
            var objectContextKey = $"{typeof(T)}-{webContext.GetHashCode():x}";

            if (webContext.HttpContext.Items.Contains(objectContextKey))
                return webContext.HttpContext.Items[objectContextKey] as T;

            T type;
            using (IKernel kernel = new StandardKernel())
            {
                kernel.Bind<T>().ToSelf().InRequestScope();
                type = kernel.Get<T>();
            }
            webContext.HttpContext.Items.Add(objectContextKey, type);
            return webContext.HttpContext.Items[objectContextKey] as T;
        }

        public static T GetCurrentUser<T>(this ControllerContext webContext) where T : ApplicationUser
        {
            var context = webContext.GetDbContext<ApplicationDbContext>();
            return context.Users.Find(webContext.HttpContext.User.Identity.GetUserId()) as T;
        }
    }
}