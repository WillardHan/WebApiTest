using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApiTest.Filters;
using WebApiTest.Exceptions;
using WebApiTest.Services;

namespace WebApiTest.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;
        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet("info")]
        [CheckFilter(10,"123")]
        [DetectFilter]        
        public virtual async Task<IActionResult> info()
        {
            var test = await userService.GetByName("11");
            return Ok(test);
        }

        [HttpGet("info/{id:int}")]
        public IActionResult InfoById([FromRoute]int id)
        {
            return Ok(id);
        }

        [HttpGet("error")]
        public async Task<IActionResult> Error()
        {
            throw new ValidationException("Error Test!");
        }
    }
}
