using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WebApi.Infrastructure.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        private readonly IHostEnvironment hostEnvironment;
        private readonly ILogger<ExceptionFilter> logger;
        public ExceptionFilter(IHostEnvironment hostEnvironment, ILogger<ExceptionFilter> logger)
        {
            this.hostEnvironment = hostEnvironment;
            this.logger = logger;
        }
        public void OnException(ExceptionContext context)
        {
            logger.LogError(new EventId(context.Exception.HResult), context.Exception, context.Exception.Message);

            ContentResult result = new ContentResult
            {
                Content = $"{context.Exception.Message}\r\n{context.Exception.StackTrace}",
                StatusCode = (int)HttpStatusCode.OK,
                ContentType = "text/html;charset=utf-8"
                //ContentType = "application/json"   
            };

            if (context.Exception is ValidationException)
            {
                result.Content = context.Exception.Message ?? context.Exception.InnerException?.Message ?? "请求异常";
                result.StatusCode = (int)HttpStatusCode.BadRequest;
            }
            else if (context.Exception is SystemException)
            {
                result.Content = context.Exception.Message ?? context.Exception.InnerException?.Message ?? "系统异常";
                result.StatusCode = (int)HttpStatusCode.BadRequest;
            }
            else if (context.Exception is OperationCanceledException)
            {
                result.Content = "客户端请求已中断";
                result.StatusCode = (int)HttpStatusCode.BadRequest;
            }
            else if(context.Exception is TimeoutException)
            {
                result.Content = "服务响应处理超时";
                result.StatusCode = (int)HttpStatusCode.BadRequest;
            }
            else
            {
                if (hostEnvironment.IsDevelopment())
                {
                    result.Content = context.Exception.InnerException?.Message ?? "系统繁忙，稍后重试";
                    result.StatusCode = (int)HttpStatusCode.BadRequest;
                }
                else
                {
                    result.Content = "系统繁忙，稍后重试";
                    result.StatusCode = (int)HttpStatusCode.BadRequest;
                }
            }

            context.Result = result;
            context.ExceptionHandled = true; //Exception Handled flag
        }
    }
}
