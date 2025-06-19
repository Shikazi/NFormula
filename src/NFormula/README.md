# NFormula

**NFormula** is a lightweight .NET formula expression engine that allows you to parse and evaluate dynamic expressions written by end users.  
It supports custom variables (`{{...}}`), pluggable functions and operators, and is built with extensibility and performance in mind.

---

## ✨ Features

- Supports arithmetic, logical, and comparison operators
- Allows user-defined functions and operators
- Manual tokenizer (no regex dependency)
- Parses variables like `{{variableName}}`
- Perfect for rule engines, custom calculations, and expression-based workflows

---

## 📦 Installation

```bash
dotnet add package NFormula
```

---

## 🚀 Quick Start

```csharp
var parser = new NFormularParserBuilder()
    .AddDefaultFunctions()
    .AddDefaultOperators()
    .Build();

var expr = parser.Parse("1 + 2 * 3");
var result = expr.Evaluate(); // result = 7
```

With variable context:

```csharp
var context = new DictionaryEvaluationContext(new Dictionary<string, object>
{
    { "x", 10 },
    { "y", 5 }
});

var expr = parser.Parse("{{x}} - {{y}}");
var result = expr.Evaluate(context); // result = 5
```

---

## 🧩 Extensibility

You can register your own:

- `IFunction` implementations
- `IBinaryOperator` or `IUnaryOperator` implementations

This allows you to fully customize your formula domain.

---

## 📚 Use Cases

- Business rule engines
- Dynamic forms or workflow conditions
- Spreadsheet-like formula evaluation
- Lightweight expression system
