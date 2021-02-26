using System;

namespace WebApi.Infrastructure.Exceptions
{
    public class SystemLevelException : Exception
    {
        public SystemLevelException()
        { 
        
        }

        public SystemLevelException(string message, Exception exception) : base(message, exception)
        {

        }
    }
}
