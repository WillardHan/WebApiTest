using System;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace WebApi.Infrastructure.Attributes
{
    public class CheckActionFilterAttribute : Attribute, IActionFilter
    {
        private readonly ILogger<CheckActionFilterAttribute> logger;
        public int ParaA { get; set; }
        public string ParaB { get; set; }
        public CheckActionFilterAttribute(ILogger<CheckActionFilterAttribute> logger, int paraA, string paraB)
        {
            this.logger = logger;
            this.ParaA = paraA;
            this.ParaB = paraB;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var test = context.HttpContext.Request;
            this.logger.LogWarning("before check");
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            this.logger.LogWarning("after check");
        }
    }
}
