using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using WebApi.Infrastructure.Attributes;
using WebApi.Infrastructure.Utility;
using WebApiTest.Application.Models;

namespace WebApiTest.Controllers
{
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        //private readonly IMapper mapper;
        public TestController()
        {
        }

        [HttpGet("info")]     
        public virtual IActionResult Info()
        {
            var test1 = new Test1
            {
                Id = 1,
                Code = "2",
                Name = "3"
            };

            var test2 = test1.ToDTO<Test2>();
            var test3 = test1.ToDTO<Test2>();

            return Ok(test3);
        }

        [HttpGet("info/{val}")]
        public IActionResult InfoById([FromRoute]string val)
        {
            return Ok(val);
        }

        [HttpGet("query")]
        public IActionResult Query([FromQuery]string a, int b)
        {
            return Ok(a + b);
        }

        [HttpPost("list")]
        [NoLogger]
        public IActionResult InfoById([FromBody]List<string> list)
        {
            return Ok(list);
        }
    }
}
