# C# Refactoring Practice Project

This is a hands-on C# project where you can practice refactoring techniques from Martin Fowler's catalog.

## Getting Started

### Prerequisites

- .NET 8.0 SDK or later
- Any C# IDE (Visual Studio, VS Code, Rider, etc.)

### Running the Project

```bash
# Navigate to the project directory
cd examples/csharp-practice

# Restore dependencies
dotnet restore

# Run the console application
dotnet run

# Run the tests (from the test project directory)
cd ../csharp-tests
dotnet test
```

## What's Included

### Program.cs

Contains before/after examples of common refactoring patterns:

1. **Extract Method** - Breaking down a long method into smaller, focused methods
2. **Replace Magic Number with Constant** - Using named constants instead of literal values
3. **Guard Clauses** - Early returns to reduce nesting

### ../csharp-tests/RefactoringTests.cs

Unit tests that verify the refactored code works correctly. These tests demonstrate:

- Testing extracted methods independently
- Using theory tests for multiple scenarios
- Exception testing for edge cases

## Practice Exercises

### Exercise 1: Extract Method

Look at the `PrintOwing` method in Program.cs. It's doing too many things:

- Printing a banner
- Calculating outstanding amount
- Printing details

**Your task**: Refactor it using the Extract Method pattern (see `PrintOwingRefactored` for the solution).

### Exercise 2: Replace Magic Numbers

The `CalculateDiscountBad` method uses magic numbers (65, 18, 0.15, etc.).

**Your task**: Replace these with named constants (see `CalculateDiscount` for the solution).

### Exercise 3: Add Guard Clauses

Create a method that validates customer data and uses guard clauses for early returns.

### Exercise 4: Encapsulate Field

Add a `CreditLimit` property to the `Account` class and encapsulate it properly.

## Running Your Own Experiments

1. Copy any code example from the main catalog (docs/Refactoring-Catalog.md)
2. Paste it into Program.cs
3. Write tests for it in RefactoringTests.cs
4. Run `dotnet test` to verify your refactoring doesn't break functionality
5. Run `dotnet run` to see the behavior

## Common Commands

```bash
# Build the project
dotnet build

# Run with verbose output
dotnet run --verbosity normal

# Run specific test
dotnet test --filter "MethodName"

# Watch for changes and re-run tests
dotnet watch test
```

## Tips for Practice

1. **Start small**: Pick one smell, apply one refactoring
2. **Test first**: Write tests before refactoring to ensure behavior doesn't change
3. **One step at a time**: Make the smallest possible change, then test
4. **Use the IDE**: Modern IDEs have automated refactoring tools - try them!
5. **Compare**: Look at the before/after examples to understand the improvement

## Next Steps

Once you're comfortable with these examples:

1. Browse the complete catalog: [docs/Refactoring-Catalog.md](../../docs/Refactoring-Catalog.md)
2. Try more complex refactorings like Extract Class or Replace Conditional with Polymorphism
3. Apply these techniques to your own codebase

Happy refactoring! 🔧
