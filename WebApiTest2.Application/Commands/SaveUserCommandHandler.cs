using MediatR;
using System.Threading;
using System.Threading.Tasks;
using WebApi.Infrastructure.Exceptions;
using WebApiTest2.Application.Commands;
using WebApiTest2.Domain.Models;

namespace WebApiTest2.Commands
{
    public class SaveComputerCommandHandler : IRequestHandler<SaveUserCommand, bool>
    {
        private readonly IUserRepository repository;
        public SaveComputerCommandHandler(
            IUserRepository repository
            )
        {
            this.repository = repository;
        }

        public async Task<bool> Handle(SaveUserCommand request, CancellationToken cancellationToken)
        {
            if (!request.Id.HasValue)
            {
                if (repository.IsCodeExist(request.Code)) throw new ValidateLevelException("该电脑编码已存在");
                repository.Add(new User(request.Code, request.Name));
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
