# Refactoring in C#: Practical Guide and Catalog

[![Language](https://img.shields.io/badge/language-C%23-178600)](#) [![Focus](https://img.shields.io/badge/focus-refactoring-blue)](#) [![Docs](https://img.shields.io/badge/docs-book_style-green)](#)

This repository is a hands‑on companion to Martin Fowler's Refactoring, adapted with modern C# examples. Use it to learn, teach, and quickly apply refactorings to real code.

## Quick Navigation

- **Complete Catalog**: [docs/Refactoring-Catalog.md](docs/Refactoring-Catalog.md) - Full reference with all 74 refactoring techniques
- **Bad Smells**: [docs/Refactoring-Catalog.md#3-bad-smells-in-code](docs/Refactoring-Catalog.md#3-bad-smells-in-code) - Spot issues in code fast
- **Most-used refactorings**: Extract Method, Replace Temp with Query, Guard Clauses, Polymorphism
- **New in 2nd edition**: Split Phase, Replace Loop with Pipeline, Slide Statements

## How to Use This Guide

- Pick a smell → choose a refactoring → apply the smallest step → run tests
- Prefer clear names, small methods, no magic numbers, SOLID principles
- Copy/paste examples as templates and adapt to your domain

## Contents (Book-Style Walkthrough)

- [Foreword to the First Edition](#foreword-to-the-first-edition)
- [Preface](#preface)
- [Chapter 1: Refactoring: A First Example](#chapter-1-refactoring-a-first-example)
- [Chapter 2: Principles in Refactoring](#chapter-2-principles-in-refactoring)
- [Chapter 3: Bad Smells in Code](#chapter-3-bad-smells-in-code)
- [Chapter 4: Building Tests](#chapter-4-building-tests)
- [Chapter 5: Introducing the Catalog](#chapter-5-introducing-the-catalog)
- [Chapter 6: A First Set of Refactorings](#chapter-6-a-first-set-of-refactorings)
- [Chapter 7: Encapsulation](#chapter-7-encapsulation)
- [Chapter 8: Moving Features](#chapter-8-moving-features)
- [Chapter 9: Organizing Data](#chapter-9-organizing-data)
- [Chapter 10: Simplifying Conditional Logic](#chapter-10-simplifying-conditional-logic)
- [Chapter 11: Refactoring APIs](#chapter-11-refactoring-apis)
- [Chapter 12: Dealing with Inheritance](#chapter-12-dealing-with-inheritance)

## Attribution & Rights

- This is a learning aid. Read the book to master the material.
- Source material: Refactoring by Martin Fowler (Addison‑Wesley)
- Publisher note: if you believe this repo should not be public, contact me through github issue and it will be made private.

Contributions welcome — issues and PRs help make this better.

---

## Foreword to the First Edition

Refactoring improves design without changing behavior. This guide focuses on disciplined, incremental changes in C#.

## Preface

Use small, verified steps, backed by tests. Prefer readability, SOLID, and clear naming.

---

## Chapter 1: Refactoring: A First Example

Goal: Separate calculation from presentation, reduce duplication, and introduce small, testable functions. We’ll follow the flow in Fowler’s example using concise C#.

### The Starting Point

One function that calculates, formats, and prints:

```csharp
string Statement(Order order) {
    var total = 0m;
    var volumeCredits = 0;
    var result = $"Statement for {order.Customer}\n";
    foreach (var line in order.Lines) {
        var amount = line.Quantity * line.UnitPrice; // inline calc
        total += amount;
        if (line.Quantity > 10) volumeCredits += (line.Quantity - 10); // mixed concern
        result += $"  {line.ProductName}: {amount:C} ({line.Quantity})\n"; // formatting here
    }
    result += $"Amount owed is {total:C}\n";
    result += $"You earned {volumeCredits} credits\n";
    return result;
}
```

Problems: mixed concerns, hard to unit test, duplication risk.

### The First Step in Refactoring: Extract Small Functions

Make the calculation intent obvious; isolate behaviors:

```csharp
decimal AmountFor(OrderLine line) => line.Quantity * line.UnitPrice;
int VolumeCreditsFor(OrderLine line) => Math.Max(line.Quantity - 10, 0);
```

### Decomposing the Function

```csharp
string Statement(Order order) {
    var total = 0m; var credits = 0; var sb = new StringBuilder();
    sb.AppendLine($"Statement for {order.Customer}");
    foreach (var line in order.Lines) {
        var amount = AmountFor(line);
        total += amount; credits += VolumeCreditsFor(line);
        sb.AppendLine($"  {line.ProductName}: {amount:C} ({line.Quantity})");
    }
    sb.AppendLine($"Amount owed is {total:C}");
    sb.AppendLine($"You earned {credits} credits");
    return sb.ToString();
}
```

Still mixes calculation with formatting.

### Split the Phases: Calculation vs. Rendering

Create an intermediate data structure for rendering:

```csharp
record LineView(string Product, int Qty, decimal Amount);
record StatementData(string Customer, IReadOnlyList<LineView> Lines, decimal Total, int Credits);

StatementData Calculate(Order order) {
    var lines = order.Lines
        .Select(l => new LineView(l.ProductName, l.Quantity, AmountFor(l)))
        .ToList();
    var total = lines.Sum(l => l.Amount);
    var credits = order.Lines.Sum(VolumeCreditsFor);
    return new StatementData(order.Customer, lines, total, credits);
}

string RenderPlainText(StatementData data) {
    var sb = new StringBuilder();
    sb.AppendLine($"Statement for {data.Customer}");
    foreach (var l in data.Lines)
        sb.AppendLine($"  {l.Product}: {l.Amount:C} ({l.Qty})");
    sb.AppendLine($"Amount owed is {data.Total:C}");
    sb.AppendLine($"You earned {data.Credits} credits");
    return sb.ToString();
}
```

Now calculation can be tested independent of formatting.

### Reorganize Calculations by Type (Polymorphic Calculator)

When pricing varies by type, use polymorphism instead of conditionals:

```csharp
abstract class PricingRule { public abstract decimal AmountFor(OrderLine l); }
class StandardPricing : PricingRule { public override decimal AmountFor(OrderLine l) => l.Quantity * l.UnitPrice; }
class PremiumPricing : PricingRule { public override decimal AmountFor(OrderLine l) => l.Quantity * l.UnitPrice * 0.9m; }

PricingRule RuleFor(OrderLine l) => l.IsPremium ? new PremiumPricing() : new StandardPricing();
decimal AmountFor(OrderLine l) => RuleFor(l).AmountFor(l);
```

This isolates variation and reduces branching.

### Status

- Separated into two phases (Calculate → Render)
- Variation moved behind polymorphism
- Small, testable units

### Final Thoughts

Keep changes behavior‑preserving, verify with tests at each step, and iterate in small, safe increments.

## Chapter 2: Principles in Refactoring

### Defining Refactoring

Behavior‑preserving code transformations that improve design, readability, or structure. After each step, tests must pass.

### The Two Hats

- Add behavior: write new tests, implement features; tolerate duplication temporarily
- Refactor: no new behavior; remove duplication, clarify intent, improve structure

### Why Should We Refactor?

- Improves changeability and reduces defects
- Makes intent clear; lowers cognitive load for future work
- Enables simpler designs (smaller classes and functions)

### When Should We Refactor?

- Before: clarify code youre about to modify
- During: when duplication or confusion appears while adding behavior
- After: clean up once tests pass

### Problems with Refactoring

- Lacking tests increases risk  write characterization tests first
- Large, sweeping refactors are risky  prefer incremental steps
- Team discipline required: respect the hat youre wearing

### Refactoring, Architecture, and YAGNI

- Emerge design through small refactorings; avoid speculative abstractions
- YAGNI: delay generalization until a second concrete use appears

### Refactoring and the Wider Process

- Works with CI/CD, code review, and trunk‑based or short‑lived branches
- Use feature flags to keep refactors releasable

### Refactoring and Performance

- **Three approaches to performance**:
  1. **Time budgeting**: allocate time resources to components (hard to predict)
  2. **Constant attention**: optimize continuously (often counterproductive)
  3. **Performance-aware programming**: write tunable code, then optimize (recommended)
- **Refactoring enables performance**: well-factored code is easier to profile and tune
- **90% rule**: most programs spend 90% of time in 10% of code
- Profile first, optimize the bottlenecks, measure results

### Where Did Refactoring Come From?

- Kent Beck, Martin Fowler, and others documented concrete refactorings and motivations
- IDE support and unit testing made safe refactoring practical

### Automated Refactorings

Use IDE operations for behavior‑preserving steps:

- Rename symbol, Extract Method, Inline Method, Move Type/Member
- Change Signature, Introduce Variable/Field/Property

### Modern C# and Functional Approaches

The 2nd edition emphasizes less class-centric design. C# supports this with:

- **Records**: Immutable data containers with value semantics
- **Pattern matching**: Cleaner conditional logic
- **Local functions**: Nested functions for better encapsulation
- **Expression-bodied members**: Concise function definitions
- **LINQ**: Functional data transformations replacing imperative loops

### Going Further

- Keep functions small and named by intent
- Prefer immutability for clarity and testability
- Continuously improve: small steps, verified by tests

---

## Chapter 3: Bad Smells in Code

Reference the full list in [Complete Catalog](docs/Refactoring-Catalog.md#3-bad-smells-in-code). Below are concise C# illustrations and typical remedies.

### Duplicated Code 2 Extract Method

```csharp
// before (duplicated logic in multiple places)
var amount = qty * unitPrice * (1 - discount);
// after
decimal NetAmount(int qty, decimal unitPrice, decimal discount) => qty * unitPrice * (1 - discount);
```

### Long Method 2 Decompose + Guard Clauses

```csharp
// before: many branches inline
if (!customer.IsActive) {/* ... */} else {/* ... */}
// after
if (!customer.IsActive) return;
ApplyPricing(order);
SendConfirmation(order);
```

### Feature Envy 2 Move Method

```csharp
// before
decimal TotalFor(Order order) => order.Lines.Sum(l => l.Quantity * l.UnitPrice);
// after (behavior lives with the data)
class Order { public decimal Total() => Lines.Sum(l => l.Quantity * l.UnitPrice); }
```

### Data Clumps 2 Introduce Parameter Object

```csharp
// before
Ship(street, city, state, postalCode);
// after
record Address(string Street, string City, string State, string PostalCode);
Ship(new Address(street, city, state, postalCode));
```

### Primitive Obsession 2 Replace Primitive with Object

```csharp
public readonly struct Money {
    public decimal Amount { get; }
    public string Currency { get; }
    public Money(decimal amount, string currency) { Amount = amount; Currency = currency; }
    public static Money operator +(Money a, Money b) => a.Currency == b.Currency ? new(a.Amount + b.Amount, a.Currency) : throw new InvalidOperationException();
}
```

### Repeated Switches 2 Replace Conditional with Polymorphism

```csharp
abstract class PricingRule { public abstract decimal AmountFor(OrderLine l); }
sealed class Standard : PricingRule { public override decimal AmountFor(OrderLine l) => l.Quantity * l.UnitPrice; }
sealed class Premium : PricingRule { public override decimal AmountFor(OrderLine l) => l.Quantity * l.UnitPrice * 0.9m; }
```

---

## Chapter 4: Building Tests

### The Value of Self-Testing Code

- Enables safe, incremental refactoring
- Documents behavior and intent
- Catches regressions quickly

### Sample Code to Test

We separated calculation from rendering in Chapter 1. Test them independently.

### A First Test (xUnit-style)

```csharp
[Fact]
public void Calculate_Computes_Total_And_Credits() {
    var order = NewOrder(("Widget", 12, 5.00m));
    var data = Calculate(order);
    Assert.Equal(60.00m, data.Total);
    Assert.True(data.Credits > 0);
}
```

### Add Another Test (Rendering)

```csharp
[Fact]
public void RenderPlainText_Formats_Lines_And_Summary() {
    var order = NewOrder(("Widget", 2, 3.00m));
    var text = RenderPlainText(Calculate(order));
    Assert.Contains("Statement for", text);
    Assert.Contains("Widget", text);
    Assert.Contains("Amount owed", text);
}
```

### Modifying the Fixture (Factory/Builder)

```csharp
static Order NewOrder(params (string name, int qty, decimal price)[] lines) =>
    new Order {
        Customer = "Acme",
        Lines = lines.Select(l => new OrderLine { ProductName = l.name, Quantity = l.qty, UnitPrice = l.price }).ToList()
    };
```

### Probing the Boundaries (Edge Cases)

```csharp
[Theory]
[InlineData(0)]
[InlineData(-1)]
public void GetValueForPeriod_OutsideBounds_ReturnsZero(int period) {
    var value = GetValueForPeriod(period);
    Assert.Equal(0, value);
}

[Fact]
public void Withdraw_OverBalance_Throws() {
    Assert.Throws<BalanceException>(() => Withdraw(int.MaxValue));
}
```

### Much More Than This

- Start small, cover hot spots, evolve tests alongside refactoring
- Prefer fast, deterministic tests; isolate external effects

---

## Chapter 5: Introducing the Catalog

Format: intent, motivation, mechanics, and small C# examples. This guide links book naming to classic catalog entries in the repo.

### Refactoring Record Format

Each refactoring follows this structure:

- **Name**: Clear, intention-revealing name
- **Sketch**: Visual before/after code comparison
- **Motivation**: When and why to apply this refactoring
- **Mechanics**: Step-by-step instructions
- **Example**: Detailed walkthrough with C# code

### Building a Vocabulary

Teams benefit from shared refactoring vocabulary. Instead of "clean up this method," say "extract method" or "replace temp with query."

### The Role of Tests

Tests are essential safety net. Red-green-refactor cycle:

1. **Red**: Write failing test
2. **Green**: Make it pass (quick and dirty)
3. **Refactor**: Clean up while keeping tests green

### What's New in the 2nd Edition

Key additions and changes from Fowler's 2nd edition:

- **Less class-centric**: Emphasizes functions and composition over inheritance
- **Modern language features**: Uses C# records, LINQ, nullable reference types
- **New refactorings**: Split Phase, Replace Loop with Pipeline, Slide Statements
- **Updated examples**: All examples rewritten for clarity and modern practices
- **Functional approach**: More emphasis on immutable data and pure functions

---

## Chapter 6: A First Set of Refactorings

Name mapping (book → classic/catalog):

- Extract Function → [Extract Method](docs/Refactoring-Catalog.md#1-extract-method)
- Inline Function → [Inline Method](docs/Refactoring-Catalog.md#2-inline-method)
- Extract Variable → [Introduce Explaining Variable](docs/Refactoring-Catalog.md#5-introduce-explaining-variable)
- Inline Variable → [Inline Temp](docs/Refactoring-Catalog.md#3-inline-temp)
- Change Function Declaration → [Rename method / Add/Remove Parameter](docs/Refactoring-Catalog.md#41-rename-method)
- Introduce Parameter Object → [Introduce Parameter Object](docs/Refactoring-Catalog.md#49-introduce-parameter-object)
- Combine Functions into Class → [Extract Class](docs/Refactoring-Catalog.md#12-extract-class)
- Combine Functions into Transform → [Replace Method with Method Object](docs/Refactoring-Catalog.md#8-replace-method-with-method-object)
- Split Phase → [Split Phase](docs/Refactoring-Catalog.md#)

Example – Extract Function (Extract Method):

```csharp
double TotalPrice(Order order) => order.Lines.Sum(l => l.Total);
```

Example – Inline Function (Inline Method):

```csharp
// before: double BasePrice() => quantity * itemPrice;
var basePrice = quantity * itemPrice;
```

---

## Chapter 7: Encapsulation

- Encapsulate Record → [Replace Data Value with Object](docs/Refactoring-Catalog.md#19-replace-data-value-with-object)
- Encapsulate Collection → [Encapsulate Collection](docs/Refactoring-Catalog.md#28-encapsulate-collection)
- Replace Primitive with Object → [Replace Data Value with Object](docs/Refactoring-Catalog.md#19-replace-data-value-with-object)
- Replace Temp with Query → [Replace Temp with Query](docs/Refactoring-Catalog.md#4-replace-temp-with-query)
- Extract Class / Inline Class → [12 / 13](docs/Refactoring-Catalog.md#12-extract-class)
- Hide Delegate / Remove Middle Man → [14 / 15](docs/Refactoring-Catalog.md#14-hide-delegate)
- Substitute Algorithm → [9](docs/Refactoring-Catalog.md#9-substitute-algorithm)

Encapsulate Field:

```csharp
private string _name;
public string Name { get { return _name; } set { _name = value; } }
```

Encapsulate Collection:

```csharp
private readonly HashSet<Course> _courses = new();
public IReadOnlyCollection<Course> Courses => _courses;
public void AddCourse(Course c) => _courses.Add(c);
public void RemoveCourse(Course c) => _courses.Remove(c);
```

---

## Chapter 8: Moving Features

- Move Function / Field → [10 / 11](docs/Refactoring-Catalog.md#10-move-method)
- Move Statements into Function / to Callers → see method extraction examples
- Replace Inline Code with Function Call → [16](docs/Refactoring-Catalog.md#16-introduce-foreign-method)
- Slide Statements / Split Loop / Replace Loop with Pipeline → use LINQ in C#
- Remove Dead Code → delete unreachable/unused members

Example – Replace Loop with Pipeline (LINQ):

```csharp
var total = orders.Where(o => o.IsActive).Sum(o => o.Amount);
```

---

## Chapter 9: Organizing Data

- Split Variable / Rename Field
- Replace Derived Variable with Query
- Change Reference to Value / Value to Reference → overrides, identity

Replace Magic Number with Constant:

```csharp
const double GravitationalConstant = 9.81;
double PotentialEnergy(double mass, double height) => mass * GravitationalConstant * height;
```

Change Reference to Value (override equality):

```csharp
public readonly struct Money {
    public decimal Amount { get; }
    public string Currency { get; }
}
```

---

## Chapter 10: Simplifying Conditional Logic

- Decompose Conditional / Consolidate Conditional Expression
- Replace Nested Conditional with Guard Clauses
- Replace Conditional with Polymorphism
- Introduce Special Case (Null Object)
- Introduce Assertion

Guard Clauses:

```csharp
if (IsNotEligible()) return 0;
// compute
```

Polymorphism:

```csharp
abstract class Bird { public abstract double Speed(); }
class European : Bird { public override double Speed() => Base; }
```

---

## Chapter 11: Refactoring APIs

- Separate Query from Modifier
- Parameterize Function / Remove Flag Argument
- Preserve Whole Object
- Replace Parameter with Query / Replace Query with Parameter
- Remove Setting Method
- Replace Constructor with Factory Function
- Replace Function with Command / Replace Command with Function

Error Codes → Exceptions:

```csharp
void Withdraw(int amount) {
    if (amount > _balance) throw new BalanceException();
    _balance -= amount;
}
```

Replace Exception with Test:

```csharp
double GetValueForPeriod(int p) {
    if (p >= _values.Length) return 0;
    return _values[p];
}
```

---

## Chapter 12: Dealing with Inheritance

- Pull Up / Push Down members
- Replace Type Code with Subclasses/State/Strategy
- Extract/Collapse Hierarchy
- Replace Subclass/Superclass with Delegate

Pull Up Field:

```csharp
class Employee { protected string name; }
class Salesman : Employee { }
```

Extract Interface:

```csharp
public interface IBillable { decimal GetRate(); bool HasSpecialSkill(); }
public class Employee : IBillable { /* impl */ }
```

Tease Apart Inheritance:

```csharp
class Deal { PresentationStyle presentationStyle; }
class PresentationStyle { }
```

---

## How to Use This Guide

- Read a smell, pick a refactoring, apply the smallest step, run tests
- Cross-check the full examples in [Complete Catalog](docs/Refactoring-Catalog.md) for context
- Favor SOLID, small methods, clear names, no magic numbers
