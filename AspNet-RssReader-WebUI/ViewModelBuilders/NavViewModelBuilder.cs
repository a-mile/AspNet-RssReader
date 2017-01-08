using AspNet_RssReader_Domain.Entities;
using AspNet_RssReader_WebUI.Controllers;
using AspNet_RssReader_WebUI.Infrastructure;
using AspNet_RssReader_WebUI.Interfaces;
using AspNet_RssReader_WebUI.ViewModels;

namespace AspNet_RssReader_WebUI.ViewModelBuilders
{
    public class NavViewModelBuilder:IViewModelBuilder<NavController,NavViewModel>
    {
        public NavViewModel Build(NavController controller, NavViewModel viewModel)
        {
            var currentUser = controller.ControllerContext.GetCurrentUser<ApplicationUser>();

            viewModel.Sources = currentUser.Sources;
            viewModel.Categories = currentUser.Categories;

            return viewModel;
        }
    }
}