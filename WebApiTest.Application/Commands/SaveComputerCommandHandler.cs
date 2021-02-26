using MediatR;
using System.Threading;
using System.Threading.Tasks;
using WebApi.Infrastructure.Exceptions;
using WebApiTest.Application.Commands;
using WebApiTest.Domain.Models;

namespace WebApiTest.Commands
{
    public class SaveComputerCommandHandler : IRequestHandler<SaveComputerCommand, bool>
    {
        private readonly IComputerRepository repository;
        public SaveComputerCommandHandler(
            IComputerRepository repository
            )
        {
            this.repository = repository;
        }

        public async Task<bool> Handle(SaveComputerCommand request, CancellationToken cancellationToken)
        {
            if (request.Id.HasValue)
            {
                if (repository.IsCodeExist(request.Code)) throw new ValidateLevelException("该电脑编码已存在");
                repository.Add(new Computer(request.Code, request.Name, request.DepartmentId));
            }
            else
            {
                var model = repository.Get(request.Id.Value) ?? throw new ValidateLevelException("该电脑编码已存在");
                model.Update(request.Code, request.Name);
            }
            return await repository.SaveChangesAsync() > 0;
        }
    }
}
