using Moq;
using System.Collections.Generic;
using GlobalLogisticsManagementSystem.Models;
using GlobalLogisticsManagementSystem.Repositories;

namespace GLMS.Tests.Mocks
{
    public static class MockContractRepository
    {
        public static Mock<IContractRepository> GetMock()
        {
            var mock = new Mock<IContractRepository>();

            var contracts = new List<Contract>
            {
                new Contract { ContractId = 1, Status = "Active" },
                new Contract { ContractId = 2, Status = "Expired" },
                new Contract { ContractId = 3, Status = "OnHold" },
                new Contract { ContractId = 4, Status = "Draft" }
            };

            // Setup GetByIdAsync
            mock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(contracts[0]);
            mock.Setup(repo => repo.GetByIdAsync(2)).ReturnsAsync(contracts[1]);
            mock.Setup(repo => repo.GetByIdAsync(3)).ReturnsAsync(contracts[2]);
            mock.Setup(repo => repo.GetByIdAsync(4)).ReturnsAsync(contracts[3]);
            mock.Setup(repo => repo.GetByIdAsync(It.Is<int>(id => id > 4))).ReturnsAsync((Contract?)null);

            // Setup IsContractActiveAsync
            mock.Setup(repo => repo.IsContractActiveAsync(1)).ReturnsAsync(true);
            mock.Setup(repo => repo.IsContractActiveAsync(2)).ReturnsAsync(false);
            mock.Setup(repo => repo.IsContractActiveAsync(3)).ReturnsAsync(false);
            mock.Setup(repo => repo.IsContractActiveAsync(4)).ReturnsAsync(false);

            // Setup GetAllAsync
            mock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(contracts);

            // Setup AddAsync
            mock.Setup(repo => repo.AddAsync(It.IsAny<Contract>()))
                .ReturnsAsync((Contract contract) =>
                {
                    contract.ContractId = 5;
                    return contract;
                });

            return mock;
        }
    }
}