using System;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using AspNet_RssReader_WebUI.Infrastructure;
using FluentValidation.Mvc;
using System.Web;
using AspNet_RssReader_Domain.Concrete;

namespace AspNet_RssReader_WebUI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ControllerBuilder.Current.SetControllerFactory(new NinjectControllerFactory());
            FluentValidationModelValidatorProvider.Configure();
        }
        protected void Application_EndRequest(object sender, EventArgs e)
        {
            var entityContext = HttpContext.Current.Items["dbContext"] as ApplicationDbContext;
            if (entityContext != null)
                entityContext.Dispose();
        }
    }
}
