using System.Web.Mvc;
using AspNet_RssReader_WebUI.ActionResults;
using AspNet_RssReader_WebUI.FormHandlers;
using AspNet_RssReader_WebUI.ViewModels;

namespace AspNet_RssReader_WebUI.Controllers
{
    public class SourceController : BaseController
    {
        public ActionResult Create()
        {           
            return PartialView(ViewModelFactory.GetViewModel<SourceController,CreateSourceViewModel>(this));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateSourceViewModel createSourceViewModel)
        {
            return new CreateUpdateDeleteResult<CreateSourceViewModel>(
                createSourceViewModel,
                new CreateSourceFormHandler(),
                viewModel => JavaScript("location.reload(true)"),
                viewModel => PartialView(viewModel)
            );
        }        

        public ActionResult Update(string name)
        {
            var viewModel = ViewModelFactory.GetViewModel<SourceController, UpdateSourceViewModel, string>(this, name);

            if (viewModel == null)
                return HttpNotFound();

            return PartialView(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(UpdateSourceViewModel updateSourceViewModel)
        {
            return new CreateUpdateDeleteResult<UpdateSourceViewModel>(
                updateSourceViewModel,
                new UpdateSourceFormHandler(),
                viewModel => JavaScript("location.reload(true)"),
                viewModel => PartialView(viewModel)
            );
        }

        public ActionResult Delete(string name)
        {
            var viewModel = ViewModelFactory.GetViewModel<SourceController, DeleteSourceViewModel, string>(this, name);

            if (viewModel == null)
                return HttpNotFound();

            return PartialView(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(DeleteSourceViewModel deleteSourceViewModel)
        {
            return new CreateUpdateDeleteResult<DeleteSourceViewModel>(
                deleteSourceViewModel,
                new DeleteSourceFormHandler(),
                viewModel => JavaScript("location.reload(true)"),
                viewModel => PartialView(viewModel)
            );
        }
    }
}