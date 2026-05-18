using GlobalLogisticsManagementSystem.Models;

namespace GlobalLogisticsManagementSystem.Factories
{
    public interface IServiceRequestFactory
    {
        ServiceRequest CreateServiceRequest(int contractId, string description, decimal amountUSD, decimal exchangeRate);
    }
}