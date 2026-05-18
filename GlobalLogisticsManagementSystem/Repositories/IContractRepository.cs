using System.Collections.Generic;
using System.Threading.Tasks;
using GlobalLogisticsManagementSystem.Models;

namespace GlobalLogisticsManagementSystem.Repositories
{
    public interface IContractRepository
    {
        Task<IEnumerable<Contract>> GetAllAsync();
        Task<Contract?> GetByIdAsync(int id);
        Task<Contract> AddAsync(Contract contract);
        Task UpdateAsync(Contract contract);
        Task DeleteAsync(int id);
        Task<bool> IsContractActiveAsync(int contractId);
    }
}