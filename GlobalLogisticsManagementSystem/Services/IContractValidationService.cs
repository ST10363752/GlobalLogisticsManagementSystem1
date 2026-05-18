using System.Threading.Tasks;

namespace GlobalLogisticsManagementSystem.Services
{
    public interface IContractValidationService
    {
        Task<bool> CanCreateServiceRequestAsync(int contractId);
        Task<string> GetValidationErrorMessageAsync(int contractId);
    }
}