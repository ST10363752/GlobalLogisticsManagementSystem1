using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using GLMS.API;

namespace GLMS.Tests.IntegrationTests
{
    public class API_IntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public API_IntegrationTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        // ========== CLIENTS TESTS ==========

        [Fact]
        public async Task Test_GetAllClients_ReturnsOkStatus()
        {
            // Act
            var response = await _client.GetAsync("/api/clients");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Test_GetAllClients_ReturnsData()
        {
            // Act
            var response = await _client.GetAsync("/api/clients");
            var json = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.NotEmpty(json);
            Assert.Contains("TechMove Logistics", json);
        }

        [Fact]
        public async Task Test_GetClientById_ReturnsClient()
        {
            // Act
            var response = await _client.GetAsync("/api/clients/1");
            var json = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Contains("TechMove Logistics", json);
        }

        [Fact]
        public async Task Test_GetClientById_ReturnsNotFoundForInvalidId()
        {
            // Act
            var response = await _client.GetAsync("/api/clients/9999");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Test_CreateClient_ReturnsCreatedStatus()
        {
            // Arrange
            var newClient = new
            {
                clientId = 0,
                name = "Integration Test Client",
                contactDetails = "test@integration.com",
                region = "Test Region"
            };
            var content = new StringContent(JsonSerializer.Serialize(newClient), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/clients", content);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        // ========== CONTRACTS TESTS ==========

        [Fact]
        public async Task Test_GetAllContracts_ReturnsOkStatus()
        {
            // Act
            var response = await _client.GetAsync("/api/contracts");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Test_GetAllContracts_ReturnsData()
        {
            // Act
            var response = await _client.GetAsync("/api/contracts");
            var json = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.NotEmpty(json);
            Assert.Contains("contractId", json);
        }

        [Fact]
        public async Task Test_GetContractById_ReturnsContract()
        {
            // Act
            var response = await _client.GetAsync("/api/contracts/1");
            var json = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Contains("TechMove Logistics", json);
        }

        [Fact]
        public async Task Test_GetContractById_ReturnsNotFoundForInvalidId()
        {
            // Act
            var response = await _client.GetAsync("/api/contracts/9999");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Test_FilterContracts_ByStatus_ReturnsFilteredResults()
        {
            // Act
            var response = await _client.GetAsync("/api/contracts/filter?status=Active");
            var json = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Contains("Active", json);
        }

        [Fact]
        public async Task Test_FilterContracts_ByDateRange_ReturnsFilteredResults()
        {
            // Act
            var response = await _client.GetAsync("/api/contracts/filter?startDate=2025-01-01&endDate=2026-12-31");
            var json = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Test_CreateContract_ReturnsCreatedStatus()
        {
            // Arrange
            var newContract = new
            {
                contractId = 0,
                clientId = 1,
                startDate = DateTime.Now.ToString("yyyy-MM-dd"),
                endDate = DateTime.Now.AddMonths(6).ToString("yyyy-MM-dd"),
                status = "Draft",
                serviceLevel = "Standard"
            };
            var content = new StringContent(JsonSerializer.Serialize(newContract), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/contracts", content);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task Test_UpdateContractStatus_ReturnsOk()
        {
            // Arrange
            var statusUpdate = "\"Active\"";
            var content = new StringContent(statusUpdate, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PatchAsync("/api/contracts/1/status", content);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Test_UpdateNonExistentContractStatus_ReturnsNotFound()
        {
            // Arrange
            var statusUpdate = "\"Active\"";
            var content = new StringContent(statusUpdate, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PatchAsync("/api/contracts/9999/status", content);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        // ========== SERVICE REQUESTS TESTS ==========

        [Fact]
        public async Task Test_GetAllServiceRequests_ReturnsOkStatus()
        {
            // Act
            var response = await _client.GetAsync("/api/servicerequests");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Test_GetAllServiceRequests_ReturnsData()
        {
            // Act
            var response = await _client.GetAsync("/api/servicerequests");
            var json = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.NotEmpty(json);
        }

        [Fact]
        public async Task Test_GetServiceRequestById_ReturnsNotFoundForInvalidId()
        {
            // Act
            var response = await _client.GetAsync("/api/servicerequests/9999");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Test_CanCreateServiceRequest_ForActiveContract_ReturnsTrue()
        {
            // Act
            var response = await _client.GetAsync("/api/servicerequests/can-create/1");
            var json = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Contains("true", json);
        }

        [Fact]
        public async Task Test_CanCreateServiceRequest_ForExpiredContract_ReturnsFalse()
        {
            // Act
            var response = await _client.GetAsync("/api/servicerequests/can-create/2");
            var json = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Contains("false", json);
        }

        [Fact]
        public async Task Test_CanCreateServiceRequest_ForDraftContract_ReturnsFalse()
        {
            // Act
            var response = await _client.GetAsync("/api/servicerequests/can-create/4");
            var json = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Contains("false", json);
        }

        [Fact]
        public async Task Test_CreateServiceRequest_ForActiveContract_ReturnsCreated()
        {
            // Arrange
            var newRequest = new
            {
                serviceRequestId = 0,
                contractId = 1,
                description = "Integration test service request",
                amountUSD = 500,
                amountZAR = 0,
                exchangeRateUsed = 0,
                status = "Pending",
                requestDate = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss")
            };
            var content = new StringContent(JsonSerializer.Serialize(newRequest), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/servicerequests", content);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task Test_CreateServiceRequest_ForExpiredContract_ReturnsBadRequest()
        {
            // Arrange
            var newRequest = new
            {
                serviceRequestId = 0,
                contractId = 2,
                description = "This should fail",
                amountUSD = 500,
                amountZAR = 0,
                exchangeRateUsed = 0,
                status = "Pending",
                requestDate = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss")
            };
            var content = new StringContent(JsonSerializer.Serialize(newRequest), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/servicerequests", content);
            var json = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Contains("Cannot create", json);
        }

        [Fact]
        public async Task Test_CreateServiceRequest_ForDraftContract_ReturnsBadRequest()
        {
            // Arrange
            var newRequest = new
            {
                serviceRequestId = 0,
                contractId = 4,
                description = "This should fail - draft contract",
                amountUSD = 500,
                amountZAR = 0,
                exchangeRateUsed = 0,
                status = "Pending",
                requestDate = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss")
            };
            var content = new StringContent(JsonSerializer.Serialize(newRequest), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/servicerequests", content);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Test_CreateServiceRequest_MissingContract_ReturnsBadRequest()
        {
            // Arrange
            var invalidRequest = new
            {
                serviceRequestId = 0,
                contractId = 9999,
                description = "Invalid contract",
                amountUSD = 500
            };
            var content = new StringContent(JsonSerializer.Serialize(invalidRequest), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/servicerequests", content);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}