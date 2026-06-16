using Xunit;

namespace GLMS.Tests
{
    public class SimpleTest
    {
        [Fact]
        public void Test_True_IsTrue()
        {
            // Arrange
            bool value = true;

            // Act & Assert
            Assert.True(value);
        }

        [Fact]
        public void Test_OnePlusOne_EqualsTwo()
        {
            // Arrange
            int a = 1;
            int b = 1;

            // Act
            int result = a + b;

            // Assert
            Assert.Equal(2, result);
        }
    }
}