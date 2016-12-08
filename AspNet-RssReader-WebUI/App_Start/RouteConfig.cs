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
                    sourceId = (int?) null,
                    page = 1
                });

            routes.MapRoute(null, "Page{page}",
                new
                {
                    controller = "Article",
                    action = "List",
                    sourceId = (int?) null
                },
                new
                {
                    page = @"\d+"
                });

            routes.MapRoute(null, "Source{sourceId}",
                new
                {
                    controller = "Article",
                    action = "List",
                    page = 1
                },
                new
                {
                    sourceId = @"\d+"
                });

            routes.MapRoute(null, "Source{sourceId}/Page{page}",
                new
                {
                    controller = "Article",
                    action = "List"
                }, 
                new
                {
                    page = @"\d+",
                    sourceId = @"\d+"
                });

            routes.MapRoute(null, "Source{sourceId}/Edit",
                new
                {
                    controller = "Source",
                    action = "EditSource" 
                },
                new
                {
                    sourceId = @"\d+"
                });

            routes.MapRoute(null, "MarkAllAsRead",
                new
                {
                    controller = "Article",
                    action = "MarkAllAsRead",
                    sourceId = (int?)null
                });

            routes.MapRoute(null, "Source{sourceId}/MarkAllAsRead",
                new
                {
                    controller = "Article",
                    action = "MarkAllAsRead"
                },
                new
                {
                    sourceId = @"\d+"
                });

            routes.MapRoute(null, "{controller}/{action}");
        }
    }
}
