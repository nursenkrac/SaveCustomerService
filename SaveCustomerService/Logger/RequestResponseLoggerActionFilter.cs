using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace SaveCustomerService.Logger
{
    [AttributeUsage(validOn: AttributeTargets.Class | AttributeTargets.Method)]
    public class RequestResponseLoggerActionFilter : Attribute, IActionFilter
    {
        private RequestResponseLogModel GetLogModel(HttpContext context)
        {
            return context.RequestServices.GetService<IRequestResponseLogModelCreator>().LogModel;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var model = GetLogModel(context.HttpContext);
            model.RequestDateTimeUtcActionLevel = DateTime.UtcNow;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            var model = GetLogModel(context.HttpContext);
            model.ResponseDateTimeUtcActionLevel = DateTime.UtcNow;
        }
    }

    [AttributeUsage(validOn: AttributeTargets.Class | AttributeTargets.Method)]
    public class RequestResponseLoggerErrorFilter : Attribute, IExceptionFilter
    {
        private RequestResponseLogModel GetLogModel(HttpContext context)
        {

            return context.RequestServices.GetService<IRequestResponseLogModelCreator>().LogModel;
        }

        public void OnException(ExceptionContext context)
        {
            var model = GetLogModel(context.HttpContext);
            model.IsExceptionActionLevel = true;
            if (model.ResponseDateTimeUtcActionLevel == null)
            {
                model.ResponseDateTimeUtcActionLevel = DateTime.UtcNow;
            }
        }
    }
}
