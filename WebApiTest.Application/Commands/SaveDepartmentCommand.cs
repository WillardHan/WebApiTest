using MediatR;
using WebApiTest.Application.Models;

namespace WebApiTest.Application.Commands
{
    public class SaveDepartmentCommand : DepartmentRequest, IRequest<bool>
    {

    }
}
