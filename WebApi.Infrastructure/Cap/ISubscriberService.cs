using System.Threading.Tasks;

namespace WebApi.Infrastructure.Cap
{
    public interface ISubscriberService<T>
    {
        Task CheckReceivedMessage(T model);
    }
}
