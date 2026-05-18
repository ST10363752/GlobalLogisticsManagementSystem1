using Microsoft.EntityFrameworkCore;
using GlobalLogisticsManagementSystem.Data;
using System.Threading.Tasks;

namespace GlobalLogisticsManagementSystem.Services
{
    public class ContractValidationService : IContractValidationService
    {
        private readonly ApplicationDbContext _context;

        public ContractValidationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CanCreateServiceRequestAsync(int contractId)
        {
            var contract = await _context.Contracts.FindAsync(contractId);
            if (contract == null) return false;
            return contract.Status == "Active";
        }

        public async Task<string> GetValidationErrorMessageAsync(int contractId)
        {
            var contract = await _context.Contracts.FindAsync(contractId);
            if (contract == null) return "Contract not found.";
            return $"Cannot create service request. Contract is {contract.Status}. Only Active contracts are allowed.";
        }
    }
}