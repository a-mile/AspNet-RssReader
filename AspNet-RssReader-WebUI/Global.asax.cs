using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using AspNet_RssReader_WebUI.Infrastructure;
using FluentValidation.Mvc;

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
    }
}
