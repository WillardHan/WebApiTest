using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApiTest.Domain.Models
{
    public interface IOperationHistoryRepository
    {
        Task AddAsync(OperationHistory model);
        Task<List<OperationHistory>> FindAsync();
    }
}
