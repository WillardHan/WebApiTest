using DotNetCore.CAP;
using MediatR;
using System.Threading.Tasks;
using WebApi.Infrastructure.Cap;
using WebApi.Infrastructure.Cap.Dto;
using WebApi.Infrastructure.Exceptions;
using WebApiTest2.Application.Commands;

namespace WebApiTest2.Application.CapSubscriberService
{
    public class CreateDefaultUserSubscriberService : ICapSubscribe, ISubscriberService<CreateDefaultUserForDepartmentDto>
    {
        private readonly IMediator mediator;

        public CreateDefaultUserSubscriberService(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [CapSubscribe(CapConstant.CreateDefaultUserForDepartment)]
        public async Task CheckReceivedMessage(CreateDefaultUserForDepartmentDto model)
        {
            throw new ValidateLevelException("保存用户时出错");

            await mediator.Send(new SaveUserCommand
            {
                Code = model.Code,
                Name = model.Name
            });
        }
    }
}
