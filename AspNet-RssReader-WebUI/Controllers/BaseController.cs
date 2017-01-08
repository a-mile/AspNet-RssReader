using System.Web.Mvc;
using AspNet_RssReader_WebUI.Interfaces;
using AspNet_RssReader_WebUI.ViewModelBuilder;

namespace AspNet_RssReader_WebUI.Controllers
{
    public class BaseController : Controller
    {    
        protected IViewModelFactory ViewModelFactory = new ViewModelFactory();
    }
}