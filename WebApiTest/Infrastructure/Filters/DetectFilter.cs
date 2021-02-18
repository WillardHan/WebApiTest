using Microsoft.AspNetCore.Mvc;
using WebApiTest.Attributes;

namespace WebApiTest.Filters
{
    public class DetectFilter : TypeFilterAttribute
    {
        public DetectFilter() :base(typeof(DetectActionFilterAttribute))
        {
            Arguments = new object[] { };
            IsReusable = true;
            Order = 1;
        }
    }
}
