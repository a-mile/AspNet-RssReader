using System;
using AspNet_RssReader_WebUI.Interfaces;
using Ninject;
using Ninject.Extensions.Conventions;

namespace AspNet_RssReader_WebUI.ViewModelBuilder
{
    public class ViewModelFactory : IViewModelFactory
    {
        public TViewModel GetViewModel<TController, TViewModel>(TController controller)
        {
            TViewModel model;
            IViewModelBuilder<TController, TViewModel> modelBuilder;
            using (var kernel = new StandardKernel())
            {
                kernel.Bind(x => x.FromThisAssembly()
                    .SelectAllClasses()
                    .BindAllInterfaces());
                model = kernel.Get<TViewModel>();
                modelBuilder = kernel.Get<IViewModelBuilder<TController, TViewModel>>();
            }

            if (modelBuilder == null)
                throw new Exception(
                    $"Could not find a ModelBuilder with a {typeof(TController).Name} Controller/{typeof(TViewModel).Name} ViewModel pairing. Please create one.");

            return modelBuilder.Build(controller, model);
        }
        public TViewModel GetViewModel<TController, TViewModel, TInput>(TController controller, TInput data)
        {
            TViewModel model;
            IViewModelBuilder<TController, TViewModel, TInput> modelBuilder;
            using (var kernel = new StandardKernel())
            {
                kernel.Bind(x => x.FromThisAssembly()
                    .SelectAllClasses()
                    .BindAllInterfaces());
                model = kernel.Get<TViewModel>();
                modelBuilder = kernel.Get<IViewModelBuilder<TController, TViewModel, TInput>>();
            }

            if (modelBuilder == null)
                throw new Exception(
                    $"Could not find a ModelBuilder with a {typeof(TController).Name} Controller/{typeof(TViewModel).Name} ViewModel/{typeof(TInput).Name} TInput pairing. Please create one.");

            return modelBuilder.Build(controller, model, data);
        }
    }
}