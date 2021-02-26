using System;

namespace WebApi.Infrastructure.Exceptions
{
    public class ValidateLevelException : Exception
    {
        public ValidateLevelException()
        { 
        
        }

        public ValidateLevelException(string message) : base(message)
        {

        }
    }
}
