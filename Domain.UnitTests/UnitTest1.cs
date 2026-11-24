using Domain;
using Xunit;

namespace Domain.UnitTests
{
    public class DiscountCalculatorTests
    {
        [Fact]
        public void Calculate_WhenPriceIs100AndDiscountIs20_Returns80()
        {
            // Arrange
            var calculator = new DiscountCalculator();

            // Act
            var result = calculator.Calculate(100m, 20m);

            // Assert
            Assert.Equal(80m, result);
        }
    }
}