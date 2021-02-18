using Castle.DynamicProxy;

namespace WebApiTest.Interceptors
{
    public class SomeInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            //Do before...
            invocation.Proceed();
            //Do after...
        }
    }
}
