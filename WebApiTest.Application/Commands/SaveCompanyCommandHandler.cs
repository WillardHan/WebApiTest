using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using WebApiTest.Application.Commands;
using WebApiTest.Domain;
using WebApiTest.Domain.Models;

namespace WebApiTest.Commands
{
    public class SaveCompanyCommandHandler : IRequestHandler<SaveCompanyCommand, bool>
    {
        private readonly DatabaseContext databaseContext;
        public SaveCompanyCommandHandler(
            DatabaseContext databaseContext
            )
        {
            this.databaseContext = databaseContext;
        }

        public async Task<bool> Handle(SaveCompanyCommand request, CancellationToken cancellationToken)
        {
            var model = new Company
            {
                Code = request.Code,
                Name = request.Name
            };
            databaseContext.Entry(model).State = EntityState.Added;
            return await databaseContext.SaveChangesAsync() > 0;
        }
    }
}
