//using AspectCore.DependencyInjection;
//using AspectCore.DynamicProxy;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Threading.Tasks;

//namespace WebApi.Infrastructure.Attributes
//{
//    public class CustomInterceptorAttribute : AbstractInterceptorAttribute
//    {
//        [FromServiceContext]
//        public ILogger<CustomInterceptorAttribute> Logger { get; set; }
//        public async override Task Invoke(AspectContext context, AspectDelegate next)
//        {
//            this.Logger.LogWarning("before custom");
//            await next(context);
//            this.Logger.LogWarning("after custom");
//        }
//    }
//}
