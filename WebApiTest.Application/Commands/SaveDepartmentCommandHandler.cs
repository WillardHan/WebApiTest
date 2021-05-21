using DotNetCore.CAP;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebApi.Infrastructure.Cap;
using WebApi.Infrastructure.Cap.Dto;
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
        private readonly ICapPublisher capPublisher;

        public SaveDepartmentCommandHandler(
            ICompanyRepository repository,
            IMediator mediator,
            ICapPublisher capPublisher
            )
        {
            this.repository = repository;
            this.mediator = mediator;
            this.capPublisher = capPublisher;
        }

        public async Task<bool> Handle(SaveDepartmentCommand request, CancellationToken cancellationToken)
        {
            var company = repository.Get(request.CompanyId) ?? throw new ValidateLevelException("找不到该公司");
            var model = new Department(request.Code, request.Name);
            company.Departments.Add(model);

            repository.Update(company);
            await mediator.Publish(new SaveDepartmentDomainEvent(model));

            await capPublisher.PublishAsync(CapConstant.CreateDefaultUserForDepartment, new CreateDefaultUserForDepartmentDto
            {
                Code = $"default_{Guid.NewGuid()}",
                Name = "默认员工"
            });

            return await repository.SaveChangesAsync() > 0;
        }
    }
}
