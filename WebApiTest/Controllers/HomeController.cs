using Microsoft.AspNetCore.Mvc;

namespace WebApiTest.Controllers
{
    public class HomeController : ControllerBase
    {
        public IActionResult Index()
        {
            return new RedirectResult("~/swagger");
        }
    }
}
