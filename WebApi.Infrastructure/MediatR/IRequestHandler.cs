using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace WebApi.Infrastructure.MediatR
{
    public interface IRequestHandler<in TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
    }
}
