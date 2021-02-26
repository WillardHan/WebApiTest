using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebApi.Infrastructure.Exceptions;
using WebApiTest.Application.Commands;
using WebApiTest.Domain;
using WebApiTest.Domain.DomainEvents;
using WebApiTest.Domain.Models;

namespace WebApiTest.Commands
{
    public class SaveDepartmentCommandHandler : IRequestHandler<SaveDepartmentCommand, bool>
    {
        private readonly ICompanyRepository repository;
        private readonly IMediator mediator;
        public SaveDepartmentCommandHandler(
            ICompanyRepository repository,
            IMediator mediator
            )
        {
            this.repository = repository;
            this.mediator = mediator;
        }

        public async Task<bool> Handle(SaveDepartmentCommand request, CancellationToken cancellationToken)
        {
            var company = repository.Get(request.CompanyId) ??throw new ValidateLevelException("找不到该公司");
            var model = new Department(request.Code, request.Name);
            company.Departments.Add(model);

            repository.Update(company);
            await mediator.Publish(new SaveDepartmentDomainEvent(model));

            return await repository.SaveChangesAsync() > 0;
        }
    }
}
