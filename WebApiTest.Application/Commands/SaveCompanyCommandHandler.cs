using MediatR;
using System.Threading;
using System.Threading.Tasks;
using WebApi.Infrastructure.Exceptions;
using WebApiTest.Application.Commands;
using WebApiTest.Domain.Models;

namespace WebApiTest.Commands
{
    public class SaveCompanyCommandHandler : IRequestHandler<SaveCompanyCommand, bool>
    {
        private readonly ICompanyRepository repository;
        public SaveCompanyCommandHandler(
            ICompanyRepository repository
            )
        {
            this.repository = repository;
        }

        public async Task<bool> Handle(SaveCompanyCommand request, CancellationToken cancellationToken)
        {
            if (request.Id.HasValue)
            {
                if (repository.IsCodeExist(request.Code)) throw new ValidateLevelException("该公司编码已存在");
                repository.Add(new Company(request.Code, request.Name));
            }
            else
            {
                var model = repository.Get(request.Id.Value) ?? throw new ValidateLevelException("该公司编码已存在");
                model.Update(request.Code, request.Name);
            }

            return await repository.SaveChangesAsync() > 0;
        }
    }
}
