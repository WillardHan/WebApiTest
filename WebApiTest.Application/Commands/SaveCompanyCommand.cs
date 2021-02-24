using MediatR;
using WebApiTest.Application.Models;

namespace WebApiTest.Application.Commands
{
    public class SaveCompanyCommand : CompanyRequest, IRequest<bool>
    {

    }
}
