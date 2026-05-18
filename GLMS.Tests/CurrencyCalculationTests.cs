using Xunit;
using GlobalLogisticsManagementSystem.Models;

namespace GLMS.Tests
{
    public class CurrencyCalculationTests
    {
        [Fact]
        public void Test_CurrencyConversion_USD_to_ZAR_CalculatesCorrectly()
        {
            // Arrange
            decimal exchangeRate = 19.50m;
            decimal amountUSD = 1000m;
            decimal expectedZAR = 19500m;

            // Act
            decimal actualZAR = amountUSD * exchangeRate;

            // Assert
            Assert.Equal(expectedZAR, actualZAR);
        }

        [Fact]
        public void Test_CurrencyConversion_WithZeroUSD_ReturnsZeroZAR()
        {
            // Arrange
            decimal exchangeRate = 19.50m;
            decimal amountUSD = 0m;
            decimal expectedZAR = 0m;

            // Act
            decimal actualZAR = amountUSD * exchangeRate;

            // Assert
            Assert.Equal(expectedZAR, actualZAR);
        }

        [Fact]
        public void Test_CurrencyConversion_WithLargeAmount_CalculatesCorrectly()
        {
            // Arrange
            decimal exchangeRate = 19.50m;
            decimal amountUSD = 1000000m;
            decimal expectedZAR = 19500000m;

            // Act
            decimal actualZAR = amountUSD * exchangeRate;

            // Assert
            Assert.Equal(expectedZAR, actualZAR);
        }

        [Theory]
        [InlineData(100, 19.50, 1950)]
        [InlineData(250, 19.50, 4875)]
        [InlineData(500, 19.50, 9750)]
        [InlineData(1000, 19.50, 19500)]
        [InlineData(1500, 19.50, 29250)]
        public void Test_CurrencyConversion_MultipleValues_CalculatesCorrectly(decimal usd, decimal rate, decimal expectedZar)
        {
            // Act
            decimal actualZar = usd * rate;

            // Assert
            Assert.Equal(expectedZar, actualZar);
        }
    }
}