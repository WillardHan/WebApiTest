using Microsoft.AspNetCore.Mvc;
using WebApi.Infrastructure.Attributes;

namespace WebApi.Infrastructure.Filters
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
