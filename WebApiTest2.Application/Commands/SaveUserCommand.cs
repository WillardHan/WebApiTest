using MediatR;
using WebApiTest2.Application.Models;

namespace WebApiTest2.Application.Commands
{
    public class SaveUserCommand : UserRequest, IRequest<bool>
    {

    }
}
