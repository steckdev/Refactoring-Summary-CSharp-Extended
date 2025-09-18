# C# Refactoring Examples

This directory contains practical C# examples for learning and practicing refactoring techniques.

## Directory Structure

### 📁 csharp-practice/

**Hands-on practice project** - A complete C# console application with before/after refactoring examples.

- **Purpose**: Learn refactoring by modifying real, runnable code
- **What's included**: Extract Method, Replace Magic Numbers, Guard Clauses, and more
- **How to use**: Copy examples, modify them, run tests to verify behavior
- **Commands**: `dotnet run` to see examples, `cd ../csharp-tests && dotnet test` to verify

### 📁 csharp-tests/

**Unit tests** for the practice project to ensure refactorings don't break functionality.

- **Purpose**: Demonstrate test-driven refactoring
- **What's included**: xUnit tests for all refactored methods
- **How to use**: Run `dotnet test` to verify your changes work correctly

### 📁 verification/

**Compilation verification** - Ensures key catalog examples compile correctly.

- **Purpose**: Quality assurance for the documentation
- **What's included**: Representative examples from the main catalog
- **How to use**: Run `dotnet build` to verify syntax correctness

## Quick Start

1. **Start practicing**:

   ```bash
   cd csharp-practice
   dotnet run
   ```

2. **Run tests**:

   ```bash
   cd csharp-tests
   dotnet test
   ```

3. **Verify examples compile**:
   ```bash
   cd verification
   dotnet build
   ```

## Learning Path

1. **Start with Reference**: [../docs/Refactoring-Catalog.md](../docs/Refactoring-Catalog.md) - Complete catalog
2. **Practice here**: [csharp-practice/README.md](csharp-practice/README.md) - Detailed practice guide

## Requirements

- .NET 8.0 SDK or later
- Any C# IDE (Visual Studio, VS Code, Rider, etc.)

## Tips for Success

- **Start small**: Pick one smell, apply one refactoring
- **Test first**: Ensure tests pass before and after refactoring
- **One step at a time**: Make the smallest possible change
- **Use IDE tools**: Modern IDEs have automated refactoring support

Happy refactoring! 🔧
