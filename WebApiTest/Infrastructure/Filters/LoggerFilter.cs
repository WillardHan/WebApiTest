using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Linq;
using WebApiTest.Attributes;

namespace WebApiTest.Filters
{
    public class LoggerFilter : IActionFilter
    {
        private readonly ILogger<LoggerFilter> logger;
        public LoggerFilter(ILogger<LoggerFilter> logger)
        {
            this.logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (IsNoNeedLogger(context)) return;

            var test = context.HttpContext.Request;
            this.logger.LogWarning("before logger");
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (IsNoNeedLogger(context)) return;

            this.logger.LogWarning("after logger");
        }

        private bool IsNoNeedLogger(FilterContext context)
        {
            var result = false;
            if (context.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
            {
                result = controllerActionDescriptor.MethodInfo.GetCustomAttributes(inherit: true)
                                                              .Any(a => a.GetType().Equals(typeof(NoLoggerAttribute)));
            }

            return result;
        }
    }
}
