using Xunit;
using GlobalLogisticsManagementSystem.Models;

namespace GLMS.Tests
{
    public class ClientTests
    {
        [Fact]
        public void Test_Client_CreateWithValidData()
        {
            // Arrange
            var client = new Client();

            // Act
            client.Name = "Test Client";
            client.ContactDetails = "test@example.com";
            client.Region = "North America";

            // Assert
            Assert.Equal("Test Client", client.Name);
            Assert.Equal("test@example.com", client.ContactDetails);
            Assert.Equal("North America", client.Region);
        }

        [Fact]
        public void Test_Client_CanHaveMultipleContracts()
        {
            // Arrange
            var client = new Client();
            client.Contracts = new System.Collections.Generic.List<Contract>();

            // Act
            client.Contracts.Add(new Contract { ContractId = 1 });
            client.Contracts.Add(new Contract { ContractId = 2 });

            // Assert
            Assert.Equal(2, client.Contracts.Count);
        }
    }
}