using System.Linq;
using AspNet_RssReader_Domain.Entities;
using AspNet_RssReader_WebUI.Controllers;
using AspNet_RssReader_WebUI.Infrastructure;
using AspNet_RssReader_WebUI.Interfaces;
using AspNet_RssReader_WebUI.ViewModels;

namespace AspNet_RssReader_WebUI.ViewModelBuilders
{
    public class CreateSourceViewModelBuilder:IViewModelBuilder<SourceController,CreateSourceViewModel>
    {
        public CreateSourceViewModel Build(SourceController controller, CreateSourceViewModel viewModel)
        {
            return viewModel;
        }
    }

    public class UpdateSourceViewModelBuilder : IViewModelBuilder<SourceController, UpdateSourceViewModel,string>
    {
        public UpdateSourceViewModel Build(SourceController controller, UpdateSourceViewModel viewModel,string name)
        {
            var currentUser = controller.ControllerContext.GetCurrentUser<ApplicationUser>();
            var source = currentUser.Sources.FirstOrDefault(x => x.Name == name);

            if (source == null)
                return null;

            viewModel.Id = source.Id;
            viewModel.Name = source.Name;
            viewModel.Link = source.Link;
            viewModel.CategoryId = source.CategoryId;

            return viewModel;
        }
    }

    public class DeleteSourceViewModelBuilder : IViewModelBuilder<SourceController, DeleteSourceViewModel,string>
    {
        public DeleteSourceViewModel Build(SourceController controller, DeleteSourceViewModel viewModel,string name)
        {
            var currentUser = controller.ControllerContext.GetCurrentUser<ApplicationUser>();
            var source = currentUser.Sources.FirstOrDefault(x => x.Name == name);

            if (source == null)
                return null;

            viewModel.Id = source.Id;
            viewModel.Name = source.Name;

            return viewModel;
        }
    }
}