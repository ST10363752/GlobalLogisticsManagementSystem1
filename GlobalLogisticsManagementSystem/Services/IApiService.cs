using GlobalLogisticsManagementSystem.Models;

namespace GlobalLogisticsManagementSystem.Services
{
    public interface IApiService
    {
        // Clients
        Task<List<Client>> GetClientsAsync();
        Task<Client?> GetClientByIdAsync(int id);
        Task<Client> CreateClientAsync(Client client);
        Task<bool> UpdateClientAsync(int id, Client client);
        Task<bool> DeleteClientAsync(int id);

        // Contracts
        Task<List<Contract>> GetContractsAsync();
        Task<Contract?> GetContractByIdAsync(int id);
        Task<List<Contract>> FilterContractsAsync(DateTime? startDate, DateTime? endDate, string? status);
        Task<Contract> CreateContractAsync(Contract contract);
        Task<bool> UpdateContractStatusAsync(int id, string status);
        Task<bool> DeleteContractAsync(int id);

        // Service Requests
        Task<List<ServiceRequest>> GetServiceRequestsAsync();
        Task<ServiceRequest?> GetServiceRequestByIdAsync(int id);
        Task<bool> CanCreateServiceRequestAsync(int contractId);
        Task<ServiceRequest> CreateServiceRequestAsync(ServiceRequest request);
        Task<bool> UpdateServiceRequestStatusAsync(int id, string status);
        Task<bool> DeleteServiceRequestAsync(int id);
    }
}