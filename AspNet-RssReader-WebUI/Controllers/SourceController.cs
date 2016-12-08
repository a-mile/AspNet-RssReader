using System.Web.Mvc;
using AspNet_RssReader_Domain.Abstract;
using AspNet_RssReader_Domain.Entities;
using AspNet_RssReader_WebUI.ViewModels;

namespace AspNet_RssReader_WebUI.Controllers
{
    public class SourceController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public SourceController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public ViewResult AddNewSource()
        {
            return View(new AddSourceViewModel());
        }

        [HttpPost]
        public ActionResult AddNewSource(AddSourceViewModel addSource)
        {
            if (!ModelState.IsValid)
            {
                return View(addSource);
            }

            Source source = new Source
            {
                Name = addSource.Name,
                Link = addSource.Link
            };

            _unitOfWork.Repository<Source>().Add(source);
            _unitOfWork.Save();

            return RedirectToAction("List", "Article");
        }

        public ViewResult EditSource(int sourceId)
        {
            Source source = _unitOfWork.Repository<Source>().GetById(sourceId);

            EditSourceViewModel editSource = new EditSourceViewModel
            {
                Link = source.Link,
                Name = source.Name,
                SourceId = sourceId
            };

            return View(editSource);
        }

        [HttpPost]
        public ActionResult EditSource(EditSourceViewModel editSource)
        {
            if (!ModelState.IsValid)
            {
                return View(editSource);
            }

            Source source = _unitOfWork.Repository<Source>().GetById(editSource.SourceId);
            source.Name = editSource.Name;
            source.Link = editSource.Link;

            _unitOfWork.Repository<Source>().Update(source);
            _unitOfWork.Save();

            return RedirectToAction("List", "Article");
        }

        public ActionResult DeleteSource(int sourceId)
        {
            Source source = _unitOfWork.Repository<Source>().GetById(sourceId);

            DeleteSourceViewModel deleteSource = new DeleteSourceViewModel()
            {
                SourceId = sourceId,
                Name = source.Name
            };

            return View(deleteSource);
        }
        public ActionResult DeleteConfirmed(int sourceId)
        {
            _unitOfWork.Repository<Source>().Delete(sourceId);
            _unitOfWork.Save();

            return RedirectToAction("List", "Article");
        }
    }
}