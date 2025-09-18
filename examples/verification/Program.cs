using System;
using System.Collections.Generic;
using System.Linq;

namespace CompilationTest;

// This file contains key examples from the catalog to verify they compile correctly
public class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Compilation verification successful!");
        Console.WriteLine("All key refactoring examples compile correctly.");
    }
}

// Extract Method example
public class ExtractMethodExample
{
    public void PrintOwing(double amount)
    {
        PrintBanner();
        PrintDetails(amount);
    }
    
    void PrintBanner()
    {
        Console.WriteLine("***********************");
        Console.WriteLine("***** Customer Owes ****");
        Console.WriteLine("***********************");
    }
    
    void PrintDetails(double amount)
    {
        Console.WriteLine($"name: Customer");
        Console.WriteLine($"amount: {amount}");
    }
}

// Replace Magic Number with Constant
public class MagicNumberExample
{
    const decimal GRAVITATIONAL_CONSTANT = 9.81m;
    
    public decimal PotentialEnergy(decimal mass, decimal height)
    {
        return mass * GRAVITATIONAL_CONSTANT * height;
    }
}

// Encapsulate Field
public class EncapsulateFieldExample
{
    private string _name = string.Empty;
    
    public string Name
    {
        get => _name;
        set => _name = value ?? throw new ArgumentNullException(nameof(value));
    }
}

// Guard Clauses
public class GuardClausesExample
{
    public decimal GetPayAmount(Employee employee)
    {
        if (employee.IsSeparated) return 0;
        if (employee.IsRetired) return RetiredAmount(employee);
        
        return NormalPayAmount(employee);
    }
    
    decimal RetiredAmount(Employee employee) => employee.Salary * 0.5m;
    decimal NormalPayAmount(Employee employee) => employee.Salary;
}

// Parameter Object
public record Address(string Street, string City, string State, string PostalCode);

public class ParameterObjectExample
{
    public void Ship(Address address)
    {
        Console.WriteLine($"Shipping to: {address.Street}, {address.City}");
    }
}

// Replace Conditional with Polymorphism
public abstract class Bird
{
    public abstract double GetSpeed();
}

public class European : Bird
{
    public override double GetSpeed() => GetBaseSpeed();
    protected virtual double GetBaseSpeed() => 35;
}

public class African : Bird
{
    public override double GetSpeed() => GetBaseSpeed() - GetLoadFactor();
    protected virtual double GetBaseSpeed() => 40;
    protected virtual double GetLoadFactor() => 5;
}

public class NorwegianBlue : Bird
{
    private readonly bool _isNailed;
    private readonly double _voltage;
    
    public NorwegianBlue(bool isNailed, double voltage)
    {
        _isNailed = isNailed;
        _voltage = voltage;
    }
    
    public override double GetSpeed()
    {
        return _isNailed ? 0 : GetBaseSpeed(_voltage);
    }
    
    private double GetBaseSpeed(double voltage) => Math.Min(24, voltage * 10);
}

// Exception handling
public class ExceptionExample
{
    public void Withdraw(decimal amount)
    {
        if (amount < 0)
            throw new ArgumentException("Amount cannot be negative");
        
        // Process withdrawal
    }
    
    public bool CanWithdraw(decimal amount)
    {
        return amount >= 0;
    }
}

// Supporting classes
public class Employee
{
    public bool IsSeparated { get; set; }
    public bool IsRetired { get; set; }
    public decimal Salary { get; set; }
}
