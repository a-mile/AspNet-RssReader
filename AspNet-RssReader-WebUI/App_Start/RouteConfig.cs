using System.Web.Mvc;
using System.Web.Routing;

namespace AspNet_RssReader_WebUI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(null, "",
                new
                {
                    controller = "Article",
                    action = "List",
                    sourceName = (string)null,
                    categoryName = (string)null
                });

            routes.MapRoute(null, "Source/{sourceName}",
                new
                {
                    controller = "Article",
                    action = "List",
                    categoryName = (string)null
                });
            routes.MapRoute(null, "Category/{categoryName}",
                new
                {
                    controller = "Article",
                    action = "List",
                    sourceName = (string)null
                });
            routes.MapRoute(null, "DeleteSource/{sourceName}",
                new
                {
                    controller = "Source",
                    action = "DeleteSource",
                    categoryName = (string)null
                });
            routes.MapRoute(null, "DeleteCategory/{categoryName}",
                new
                {
                    controller = "Category",
                    action = "DeleteCategory",
                    sourceName = (string)null
                });
            routes.MapRoute(null, "EditSource/{sourceName}",
                new
                {
                    controller = "Source",
                    action = "EditSource",
                    categoryName = (string)null
                });
            routes.MapRoute(null, "EditCategory/{categoryName}",
                new
                {
                    controller = "Category",
                    action = "EditCategory",
                    sourceName = (string)null
                });

            routes.MapRoute(null, "MarkAllAsRead",
                new
                {
                    controller = "Article",
                    action = "MarkAllAsRead",
                    sourceName = (string)null,
                    categoryName = (string)null
                });

            routes.MapRoute(null, "MarkAllAsRead/Source/{sourceName}",
                new
                {
                    controller = "Article",
                    action = "MarkAllAsRead",
                    categoryName = (string)null
                });
            routes.MapRoute(null, "MarkAllAsRead/Category/{categoryName}",
                new
                {
                    controller = "Article",
                    action = "MarkAllAsRead",
                    sourceName = (string)null
                });
            routes.MapRoute(null, "AddNewSource",
                new
                {
                    controller = "Source",
                    action = "AddNewSource"
                });
            routes.MapRoute(null, "AddNewCategory",
                new
                {
                    controller = "Category",
                    action = "AddNewCategory"
                });

            routes.MapRoute(null, "{controller}/{action}");
        }
    }
}
