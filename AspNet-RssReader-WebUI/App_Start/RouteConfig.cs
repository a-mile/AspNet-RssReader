using System.Web.Mvc;
using System.Web.Routing;

namespace AspNet_RssReader_WebUI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default",                                              
                "{controller}/{action}/{id}",                          
                new { controller = "Article", action = "List", id = "" }  
            );
        }
    }
}
