namespace AspNet_RssReader_WebUI.Interfaces
{
    public interface IViewModelFactory
    {
        TViewModel GetViewModel<TController, TViewModel>(TController controller);
        TViewModel GetViewModel<TController, TViewModel, TInput>(TController controller, TInput data);
    }
}
