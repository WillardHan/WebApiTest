using MediatR;
using WebApiTest.Application.Models;

namespace WebApiTest.Application.Commands
{
    public class SaveComputerCommand : ComputerRequest, IRequest<bool>
    {

    }
}
