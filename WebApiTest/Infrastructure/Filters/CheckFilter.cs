using Microsoft.AspNetCore.Mvc;
using WebApiTest.Attributes;

namespace WebApiTest.Filters
{
    public class CheckFilter : TypeFilterAttribute
    {
        public CheckFilter(int paraA, string paraB) :base(typeof(CheckActionFilterAttribute))
        {
            Arguments = new object[] { paraA, paraB };
            IsReusable = true;
            Order = 2;
        }
    }
}
