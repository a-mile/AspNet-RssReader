using System.Web;
using AspNet_RssReader_Domain.Concrete;
using AspNet_RssReader_Domain.Entities;
using Microsoft.AspNet.Identity;

namespace AspNet_RssReader_WebUI.Extensions
{
    public static class HttpContextExtensions
    {
        public static ApplicationDbContext GetDbContext(this HttpContext webContext)
        {
            if (!HttpContext.Current.Items.Contains("dbContext"))
            {
                HttpContext.Current.Items.Add("dbContext", new ApplicationDbContext());
            }
            return HttpContext.Current.Items["dbContext"] as ApplicationDbContext;
        }

        public static ApplicationUser GetCurrentUser(this HttpContext webContext)
        {
            var context = webContext.GetDbContext();
            return context.Users.Find(webContext.User.Identity.GetUserId());
        }
    }
}