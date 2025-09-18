using System.Collections.Generic;
using RefactoringPractice;
using Xunit;

namespace RefactoringPractice.Tests;

public class RefactoringTests
{
    [Fact]
    public void CalculateOutstanding_WithMultipleLines_ReturnsCorrectTotal()
    {
        // Arrange
        var order = new Order
        {
            Customer = "Test Customer",
            Lines = new List<OrderLine>
            {
                new() { ProductName = "Widget", Quantity = 2, UnitPrice = 10.00m },
                new() { ProductName = "Gadget", Quantity = 1, UnitPrice = 15.00m }
            }
        };

        // Act
        var result = Program.CalculateOutstanding(order);

        // Assert
        Assert.Equal(35.00m, result);
    }

    [Theory]
    [InlineData(70, 0.15)] // Senior discount
    [InlineData(25, 0.10)] // Adult discount
    [InlineData(16, 0.05)] // Youth discount
    public void CalculateDiscount_WithDifferentAges_ReturnsCorrectDiscount(int age, decimal expectedDiscount)
    {
        // Arrange
        var customer = new Customer { Name = "Test", Age = age };

        // Act
        var result = Program.CalculateDiscount(customer);

        // Assert
        Assert.Equal(expectedDiscount, result);
    }

    [Fact]
    public void Account_Withdraw_ValidAmount_UpdatesBalance()
    {
        // Arrange
        var account = new Account { Balance = 1000m };

        // Act
        account.Withdraw(100m);

        // Assert
        Assert.Equal(900m, account.Balance);
    }

    [Fact]
    public void Account_Withdraw_ExceedsOverdraftLimit_ThrowsException()
    {
        // Arrange
        var account = new Account { Balance = 0m };

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => account.Withdraw(600m));
    }

    [Fact]
    public void Account_Withdraw_NegativeAmount_ThrowsException()
    {
        // Arrange
        var account = new Account { Balance = 1000m };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => account.Withdraw(-50m));
    }
}
