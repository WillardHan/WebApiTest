using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using WebApiTest2.Application.Services;
using WebApiTest2.Application.Commands;
using WebApi.Infrastructure.Controller;

namespace WebApiTest2.Controllers
{
    public class UserController : ApiController
    {
        private readonly IUserService userService;
        private readonly IMediator mediator;
        public UserController(
            IUserService userService,
            IMediator mediator
            )
        {
            this.userService = userService;
            this.mediator = mediator;
        }

        [HttpGet()]
        public async Task<IActionResult> Get()
        {
            return Ok(await userService.GetAll());
        }

        [HttpPut()]
        public async Task<IActionResult> Save([FromBody]SaveUserCommand command)
        {
            return Ok(await mediator.Send(command));
        }
    }
}
