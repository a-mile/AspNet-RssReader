using System.Web.Mvc;
using AspNet_RssReader_WebUI.ViewModels;

namespace AspNet_RssReader_WebUI.Controllers
{
    public class NavController : BaseController
    {
        [ChildActionOnly]
        public PartialViewResult Menu()
        {
            return PartialView(ViewModelFactory.GetViewModel<NavController, NavViewModel>(this));
        }
    }
}