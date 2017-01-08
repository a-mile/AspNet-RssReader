using System;
using System.Web.Mvc;
using AspNet_RssReader_WebUI.Interfaces;

namespace AspNet_RssReader_WebUI.ActionResults
{
    public class CreateUpdateDeleteResult<T> : ActionResult
    {
        private readonly T _model;
        public IGeneralFormHandler<T> Handler { get; set; }
        public Func<T, ActionResult> SuccessResult;
        public Func<T, ActionResult> FailureResult;
        public CreateUpdateDeleteResult(T model)
        {
            _model = model;
        }
        public CreateUpdateDeleteResult(T model,
            IGeneralFormHandler<T> handler,
            Func<T, ActionResult> successResult,
            Func<T, ActionResult> failureResult)
        {
            _model = model;
            Handler = handler;
            SuccessResult = successResult;
            FailureResult = failureResult;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var viewData = context.Controller.ViewData;
            if (viewData.ModelState.IsValid)
            {
                Handler.ProcessForm(context, _model);
                SuccessResult(_model).ExecuteResult(context);
            }
            else
            {
                FailureResult(_model).ExecuteResult(context);
            }
        }
    }
}