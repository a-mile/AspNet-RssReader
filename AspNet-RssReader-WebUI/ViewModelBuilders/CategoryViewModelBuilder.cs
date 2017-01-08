using System.Linq;
using AspNet_RssReader_Domain.Entities;
using AspNet_RssReader_WebUI.Controllers;
using AspNet_RssReader_WebUI.Infrastructure;
using AspNet_RssReader_WebUI.Interfaces;
using AspNet_RssReader_WebUI.ViewModels;

namespace AspNet_RssReader_WebUI.ViewModelBuilders
{
    public class CreateCategoryViewModelBuilder : IViewModelBuilder<CategoryController,CreateCategoryViewModel>
    {
        public CreateCategoryViewModel Build(CategoryController controller, CreateCategoryViewModel viewModel)
        {            
            return viewModel;
        }
    }

    public class UpdateCategoryViewModelBuilder : IViewModelBuilder<CategoryController, UpdateCategoryViewModel, string>
    {
        public UpdateCategoryViewModel Build(CategoryController controller, UpdateCategoryViewModel viewModel, string name)
        {
            var currentUser = controller.ControllerContext.GetCurrentUser<ApplicationUser>();
            var category = currentUser.Categories.FirstOrDefault(x => x.Name == name);

            if (category == null)
                return null;

            viewModel.Id = category.Id;
            viewModel.Name = category.Name;            

            return viewModel;
        }
    }

    public class DeleteCategoryViewModelBuilder : IViewModelBuilder<CategoryController, DeleteCategoryViewModel, string>
    {
        public DeleteCategoryViewModel Build(CategoryController controller, DeleteCategoryViewModel viewModel, string name)
        {
            var currentUser = controller.ControllerContext.GetCurrentUser<ApplicationUser>();
            var category = currentUser.Categories.FirstOrDefault(x => x.Name == name);

            if (category == null)
                return null;

            viewModel.Id = category.Id;
            viewModel.Name = category.Name;

            return viewModel;
        }
    }
}