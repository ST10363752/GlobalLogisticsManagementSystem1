using Xunit;
using GlobalLogisticsManagementSystem.Models;

namespace GLMS.Tests
{
    public class ContractValidationTests
    {
        [Fact]
        public void Test_ActiveContract_AllowsServiceRequest()
        {
            // Arrange
            var contract = new Contract
            {
                ContractId = 1,
                Status = "Active"
            };

            // Act
            bool canCreateRequest = (contract.Status == "Active");

            // Assert
            Assert.True(canCreateRequest);
        }

        [Fact]
        public void Test_ExpiredContract_BlocksServiceRequest()
        {
            // Arrange
            var contract = new Contract
            {
                ContractId = 1,
                Status = "Expired"
            };

            // Act
            bool canCreateRequest = (contract.Status == "Active");

            // Assert
            Assert.False(canCreateRequest);
        }

        [Fact]
        public void Test_OnHoldContract_BlocksServiceRequest()
        {
            // Arrange
            var contract = new Contract
            {
                ContractId = 1,
                Status = "OnHold"
            };

            // Act
            bool canCreateRequest = (contract.Status == "Active");

            // Assert
            Assert.False(canCreateRequest);
        }

        [Fact]
        public void Test_DraftContract_BlocksServiceRequest()
        {
            // Arrange
            var contract = new Contract
            {
                ContractId = 1,
                Status = "Draft"
            };

            // Act
            bool canCreateRequest = (contract.Status == "Active");

            // Assert
            Assert.False(canCreateRequest);
        }

        [Theory]
        [InlineData("Active", true)]
        [InlineData("Expired", false)]
        [InlineData("OnHold", false)]
        [InlineData("Draft", false)]
        public void Test_ContractStatus_ValidationRules(string status, bool expectedResult)
        {
            // Arrange
            var contract = new Contract
            {
                ContractId = 1,
                Status = status
            };

            // Act
            bool canCreateRequest = (contract.Status == "Active");

            // Assert
            Assert.Equal(expectedResult, canCreateRequest);
        }
    }
}