using Xunit;
using Moq;
using System.Linq;
using System.Threading.Tasks;
using GLMS.Tests.Mocks;
using GlobalLogisticsManagementSystem.Repositories;
using GlobalLogisticsManagementSystem.Models;

namespace GLMS.Tests
{
    public class RepositoryTests
    {
        private readonly Mock<IContractRepository> _mockRepo;

        public RepositoryTests()
        {
            _mockRepo = MockContractRepository.GetMock();
        }

        [Fact]
        public async Task Test_GetContractById_ReturnsActiveContract()
        {
            // Act
            var contract = await _mockRepo.Object.GetByIdAsync(1);

            // Assert
            Assert.NotNull(contract);
            Assert.Equal("Active", contract?.Status);
        }

        [Fact]
        public async Task Test_GetContractById_ReturnsExpiredContract()
        {
            // Act
            var contract = await _mockRepo.Object.GetByIdAsync(2);

            // Assert
            Assert.NotNull(contract);
            Assert.Equal("Expired", contract?.Status);
        }

        [Fact]
        public async Task Test_GetContractById_ReturnsNullForInvalidId()
        {
            // Act
            var contract = await _mockRepo.Object.GetByIdAsync(99);

            // Assert
            Assert.Null(contract);
        }

        [Fact]
        public async Task Test_IsContractActive_ReturnsTrueForActive()
        {
            // Act
            var isActive = await _mockRepo.Object.IsContractActiveAsync(1);

            // Assert
            Assert.True(isActive);
        }

        [Fact]
        public async Task Test_IsContractActive_ReturnsFalseForExpired()
        {
            // Act
            var isActive = await _mockRepo.Object.IsContractActiveAsync(2);

            // Assert
            Assert.False(isActive);
        }

        [Fact]
        public async Task Test_IsContractActive_ReturnsFalseForOnHold()
        {
            // Act
            var isActive = await _mockRepo.Object.IsContractActiveAsync(3);

            // Assert
            Assert.False(isActive);
        }

        [Fact]
        public async Task Test_IsContractActive_ReturnsFalseForDraft()
        {
            // Act
            var isActive = await _mockRepo.Object.IsContractActiveAsync(4);

            // Assert
            Assert.False(isActive);
        }

        [Fact]
        public async Task Test_AddContract_ReturnsContractWithId()
        {
            // Arrange
            var newContract = new Contract
            {
                ClientId = 1,
                Status = "Active",
                ServiceLevel = "Premium"
            };

            // Act
            var result = await _mockRepo.Object.AddAsync(newContract);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(5, result.ContractId);
        }

        [Fact]
        public async Task Test_GetAllContracts_ReturnsListOfContracts()
        {
            // Act
            var contracts = await _mockRepo.Object.GetAllAsync();

            // Assert
            Assert.NotNull(contracts);
            Assert.Equal(4, contracts.Count());
        }
    }
}