using MediatR;
using WebApiTest.Domain.Models;

namespace WebApiTest.Domain.DomainEvents
{
    public class SaveDepartmentDomainEvent : INotification
    {
        public Department Department { get; }
        public SaveDepartmentDomainEvent(Department department)
        {
            Department = department;
        }
    }
}
