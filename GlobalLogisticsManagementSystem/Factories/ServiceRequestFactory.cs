using System;
using GlobalLogisticsManagementSystem.Models;

namespace GlobalLogisticsManagementSystem.Factories
{
    public class ServiceRequestFactory : IServiceRequestFactory
    {
        public ServiceRequest CreateServiceRequest(int contractId, string description, decimal amountUSD, decimal exchangeRate)
        {
            return new ServiceRequest
            {
                ContractId = contractId,
                Description = description,
                AmountUSD = amountUSD,
                ExchangeRateUsed = exchangeRate,
                AmountZAR = amountUSD * exchangeRate,
                Status = "Pending",
                RequestDate = DateTime.Now
            };
        }
    }
}