using System.Web.Mvc;
using AspNet_RssReader_WebUI.ActionResults;
using AspNet_RssReader_WebUI.FormHandlers;
using AspNet_RssReader_WebUI.ViewModels;

namespace AspNet_RssReader_WebUI.Controllers
{
    public class CategoryController : BaseController
    {        
        public ActionResult Create()
        {
            return PartialView(ViewModelFactory.GetViewModel<CategoryController, CreateCategoryViewModel>(this));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateCategoryViewModel createCategoryViewModel)
        {
            return new CreateUpdateDeleteResult<CreateCategoryViewModel>(
                createCategoryViewModel,
                new CreateCategoryFormHandler(),
                viewModel => JavaScript("location.reload(true)"),
                viewModel => PartialView(viewModel)
            );
        }

        public ActionResult Update(string name)
        {
            var viewModel = ViewModelFactory.GetViewModel<CategoryController, UpdateCategoryViewModel, string>(this,
                name);

            if (viewModel == null)
                return HttpNotFound();

            return PartialView(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(UpdateCategoryViewModel updateCategoryViewModel)
        {
            return new CreateUpdateDeleteResult<UpdateCategoryViewModel>(
                updateCategoryViewModel,
                new UpdateCategoryFormHandler(),
                viewModel => JavaScript("location.reload(true)"),
                viewModel => PartialView(viewModel)
            );
        }

        public ActionResult Delete(string name)
        {
            var viewModel = ViewModelFactory.GetViewModel<CategoryController, DeleteCategoryViewModel, string>(this,
                name);

            if (viewModel == null)
                return HttpNotFound();

            return PartialView(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(DeleteCategoryViewModel deleteCategoryViewModel)
        {
            return new CreateUpdateDeleteResult<DeleteCategoryViewModel>(
                deleteCategoryViewModel,
                new DeleteCategoryFormHandler(),
                viewModel => JavaScript("location.reload(true)"),
                viewModel => PartialView(viewModel)
            );
        }
    }
}