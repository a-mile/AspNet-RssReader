using System.Web.Mvc;
using AspNet_RssReader_Domain.Abstract;
using AspNet_RssReader_Domain.Entities;
using AspNet_RssReader_WebUI.ViewModels;

namespace AspNet_RssReader_WebUI.Controllers
{
    public class NavController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public NavController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public PartialViewResult Menu(int? sourceId = null)
        {           
            SourcesListViewModel sourcesList = new SourcesListViewModel
            {
                Sources = _unitOfWork.Repository<Source>().SelectAll(),
                CurrentSourceId = sourceId
            };
            return PartialView(sourcesList);
        }
    }
}