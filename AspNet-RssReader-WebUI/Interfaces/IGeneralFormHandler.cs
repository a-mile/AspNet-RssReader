using System.Web.Mvc;

namespace AspNet_RssReader_WebUI.Interfaces
{
    public interface IGeneralFormHandler<TModel>
    {
        void ProcessForm(ControllerContext context, TModel model);
    }
}