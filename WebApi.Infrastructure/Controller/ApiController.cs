using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Infrastructure.Controller
{
    [Route("[controller]")]
    [ApiController]
    //[Authorize]
    public class ApiController : ControllerBase
    {
    }
}
