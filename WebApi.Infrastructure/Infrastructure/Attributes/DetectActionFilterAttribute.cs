using System;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace WebApi.Infrastructure.Attributes
{
    public class DetectActionFilterAttribute : Attribute, IActionFilter
    {
        private readonly ILogger<DetectActionFilterAttribute> logger;
        public DetectActionFilterAttribute(ILogger<DetectActionFilterAttribute> logger)
        {
            this.logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var test = context.HttpContext.Request;
            this.logger.LogWarning("before detect");
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            this.logger.LogWarning("after detect");
        }
    }
}
