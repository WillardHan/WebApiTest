using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using WebApiTest.Application.Services;
using WebApiTest.Application.Commands;
using WebApi.Infrastructure.Controller;

namespace WebApiTest.Controllers
{
    public class CompanyController : ApiController
    {
        private readonly ICompanyService companyService;
        private readonly IMediator mediator;

        public CompanyController(
            ICompanyService companyService,
            IMediator mediator
            )
        {
            this.companyService = companyService;
            this.mediator = mediator;
        }

        [HttpGet()]
        public async Task<IActionResult> Get()
        {
            return Ok(await companyService.GetAll());
        }

        [HttpPut()]
        public async Task<IActionResult> Save([FromBody]SaveCompanyCommand command)
        {
            return Ok(await mediator.Send(command));
        }

        [HttpPut("Department")]
        public async Task<IActionResult> Save([FromBody]SaveDepartmentCommand command)
        {
            return Ok(await mediator.Send(command));
        }
    }
}
