using Castle.DynamicProxy;
using System.Reflection;
using WebApi.Infrastructure.Attributes;

namespace WebApi.Infrastructure.Interceptors
{
    public class SomeInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            var isNeedInterceptor = !invocation.MethodInvocationTarget.IsDefined(typeof(NoSomeAttribute));
            if (isNeedInterceptor)
            {
                //Do before...
            }

            invocation.Proceed();

            if (isNeedInterceptor)
            {
                //Do after...
            }
        }
    }
}
