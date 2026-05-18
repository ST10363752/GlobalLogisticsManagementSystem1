using Xunit;
using GlobalLogisticsManagementSystem.Models;
using System;

namespace GLMS.Tests
{
    public class ServiceRequestTests
    {
        [Fact]
        public void Test_ServiceRequest_HasDefaultPendingStatus()
        {
            // Arrange & Act
            var request = new ServiceRequest();

            // Assert
            Assert.Equal("Pending", request.Status);
        }

        [Fact]
        public void Test_ServiceRequest_HasDefaultRequestDate()
        {
            // Arrange & Act
            var request = new ServiceRequest();

            // Assert
            Assert.True(request.RequestDate <= DateTime.Now);
            Assert.True(request.RequestDate > DateTime.Now.AddMinutes(-1));
        }

        [Fact]
        public void Test_ServiceRequest_CanUpdateStatus()
        {
            // Arrange
            var request = new ServiceRequest();

            // Act
            request.Status = "Completed";

            // Assert
            Assert.Equal("Completed", request.Status);
        }

        [Fact]
        public void Test_ServiceRequest_StoresExchangeRate()
        {
            // Arrange
            var request = new ServiceRequest();
            decimal expectedRate = 19.50m;

            // Act
            request.ExchangeRateUsed = expectedRate;

            // Assert
            Assert.Equal(expectedRate, request.ExchangeRateUsed);
        }

        [Fact]
        public void Test_ServiceRequest_CalculatesAmountZAR()
        {
            // Arrange
            var request = new ServiceRequest();
            decimal usd = 1000m;
            decimal rate = 19.50m;
            decimal expectedZar = 19500m;

            // Act
            request.AmountUSD = usd;
            request.ExchangeRateUsed = rate;
            request.AmountZAR = request.AmountUSD * request.ExchangeRateUsed;

            // Assert
            Assert.Equal(expectedZar, request.AmountZAR);
        }
    }
}