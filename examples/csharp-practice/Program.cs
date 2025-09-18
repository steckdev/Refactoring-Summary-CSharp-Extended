using System;
using System.Collections.Generic;
using System.Linq;

namespace RefactoringPractice;

public class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Refactoring Practice Examples");
        Console.WriteLine("=============================");

        // Example 1: Extract Method
        var order = new Order
        {
            Customer = "Acme Corp",
            Lines = new List<OrderLine>
            {
                new() { ProductName = "Widget", Quantity = 5, UnitPrice = 10.00m },
                new() { ProductName = "Gadget", Quantity = 2, UnitPrice = 25.00m }
            }
        };

        PrintOwing(order);

        // Example 2: Replace Magic Number with Constant
        var account = new Account { Balance = 1000 };
        account.Withdraw(100);
        Console.WriteLine($"Balance after withdrawal: {account.Balance}");

        // Example 3: Guard Clauses
        var customer = new Customer { Name = "John Doe", Age = 25 };
        var discount = CalculateDiscount(customer);
        Console.WriteLine($"Discount for {customer.Name}: {discount:P}");
    }

    // BEFORE: Long method that needs Extract Method refactoring
    static void PrintOwing(Order order)
    {
        Console.WriteLine("***********************");
        Console.WriteLine("***** Customer Owes ****");
        Console.WriteLine("***********************");

        var outstanding = 0.0m;
        foreach (var line in order.Lines)
        {
            outstanding += line.Quantity * line.UnitPrice;
        }

        Console.WriteLine($"name: {order.Customer}");
        Console.WriteLine($"amount: {outstanding}");
    }

    // AFTER: Refactored with Extract Method
    static void PrintOwingRefactored(Order order)
    {
        PrintBanner();
        var outstanding = CalculateOutstanding(order);
        PrintDetails(order.Customer, outstanding);
    }

    static void PrintBanner()
    {
        Console.WriteLine("***********************");
        Console.WriteLine("***** Customer Owes ****");
        Console.WriteLine("***********************");
    }

    public static decimal CalculateOutstanding(Order order)
    {
        return order.Lines.Sum(line => line.Quantity * line.UnitPrice);
    }

    static void PrintDetails(string customer, decimal outstanding)
    {
        Console.WriteLine($"name: {customer}");
        Console.WriteLine($"amount: {outstanding}");
    }

    // BEFORE: Magic numbers
    static decimal CalculateDiscountBad(Customer customer)
    {
        if (customer.Age > 65) return 0.15m;
        if (customer.Age > 18) return 0.10m;
        return 0.05m;
    }

    // AFTER: Replace Magic Number with Constant
    const decimal SENIOR_DISCOUNT = 0.15m;
    const decimal ADULT_DISCOUNT = 0.10m;
    const decimal YOUTH_DISCOUNT = 0.05m;
    const int SENIOR_AGE = 65;
    const int ADULT_AGE = 18;

    public static decimal CalculateDiscount(Customer customer)
    {
        if (customer.Age > SENIOR_AGE) return SENIOR_DISCOUNT;
        if (customer.Age > ADULT_AGE) return ADULT_DISCOUNT;
        return YOUTH_DISCOUNT;
    }
}

// Supporting classes
public class Order
{
    public string Customer { get; set; } = string.Empty;
    public List<OrderLine> Lines { get; set; } = new();
}

public class OrderLine
{
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}

public class Account
{
    const decimal OVERDRAFT_LIMIT = 500m;

    public decimal Balance { get; set; }

    public void Withdraw(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Amount must be positive");

        if (Balance - amount < -OVERDRAFT_LIMIT)
            throw new InvalidOperationException("Insufficient funds");

        Balance -= amount;
    }
}

public class Customer
{
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
}
