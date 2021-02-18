using System;

namespace WebApiTest.Exceptions
{
    public class SystemException : Exception
    {
        public SystemException()
        { 
        
        }

        public SystemException(string message, Exception exception) : base(message, exception)
        {

        }
    }
}
