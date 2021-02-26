using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using WebApiTest.Domain.Models;

namespace WebApiTest.Domain.DomainEvents
{
    public class SaveDepartmentDomainEventHandler : INotificationHandler<SaveDepartmentDomainEvent>
    {
        private readonly ILogger<SaveDepartmentDomainEventHandler> logger;
        private readonly DatabaseContext databaseContext;

        public SaveDepartmentDomainEventHandler(
            ILogger<SaveDepartmentDomainEventHandler> logger,
            DatabaseContext databaseContext
            )
        {
            this.logger = logger;
            this.databaseContext = databaseContext;
        }

        public async Task Handle(SaveDepartmentDomainEvent notification, CancellationToken cancellationToken)
        {
            var model = new Computer
            {
                Code = "c1",
                Name = "初始电脑1",
                DepartmentId = notification.Department.Id
            };
            databaseContext.Add(model);
            logger.LogWarning($"Handled: {notification.Department.Name}");
        }
    }
}
