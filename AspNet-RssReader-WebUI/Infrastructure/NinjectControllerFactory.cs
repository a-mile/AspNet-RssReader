using System;
using System.Data.Entity;
using System.Web.Mvc;
using System.Web.Routing;
using AspNet_RssReader_Domain.Abstract;
using AspNet_RssReader_Domain.Concrete;
using Ninject;

namespace AspNet_RssReader_WebUI.Infrastructure
{
    public class NinjectControllerFactory : DefaultControllerFactory
    {
        private readonly IKernel _ninjectKernel;
        public NinjectControllerFactory()
        {
            _ninjectKernel = new StandardKernel();
            AddBindings();
        }
        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            return controllerType == null ? null : (IController)_ninjectKernel.Get(controllerType);
        }
        private void AddBindings()
        {
            _ninjectKernel.Bind<IArticleDownloader>().To<XmlArticleDownloader>();
            _ninjectKernel.Bind<DbContext>().To<ApplicationDbContext>();

        }
    }
}