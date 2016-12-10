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
                });

            routes.MapRoute(null, "{SourceName}",
                new
                {
                    controller = "Article",
                    action = "List",
                });

  
            routes.MapRoute(null, "Delete/{sourceName}",
                new
                {
                    controller = "Source",
                    action = "DeleteSource" 
                });

            routes.MapRoute(null, "MarkAllAsRead",
                new
                {
                    controller = "Article",
                    action = "MarkAllAsRead",
                    sourceName = (string)null
                });

            routes.MapRoute(null, "MarkAllAsRead/{sourceName}",
                new
                {
                    controller = "Article",
                    action = "MarkAllAsRead"
                });

            routes.MapRoute(null, "{controller}/{action}");
        }
    }
}
