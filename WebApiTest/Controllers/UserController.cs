﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using WebApiTest.Application.Services;
using WebApi.Infrastructure.Filters;
using WebApi.Infrastructure.Attributes;
using WebApi.Infrastructure.Exceptions;
using WebApi.Infrastructure.Controller;

namespace WebApiTest.Controllers
{
    public class UserController : ApiController
    {
        private readonly IUserService userService;
        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet("info")]
        [CheckFilter(10,"123")]
        public virtual async Task<IActionResult> info(CancellationToken cancellationToken)
        {
            var test = await userService.GetByName("11", cancellationToken);
            return Ok(test);
        }

        [HttpGet("info/{id:int}")]
        [NoSome]
        public IActionResult InfoById([FromRoute]int id)
        {
            return Ok(id);
        }

        [HttpGet("error")]
        public async Task<IActionResult> Error()
        {
            throw new ValidateLevelException("Error Test!");
        }
    }
}
