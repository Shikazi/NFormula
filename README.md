
# 🧠 NFormula

[![NuGet](https://img.shields.io/nuget/v/NFormula.svg?style=flat-square)](https://www.nuget.org/packages/NFormula/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/NFormula.svg?style=flat-square)](https://www.nuget.org/packages/NFormula/)
---

**NFormula** is a powerful .NET library for parsing, evaluating, and extending user-defined expressions. It is designed for high extensibility and performance, making it ideal for rule engines, dynamic formulas, workflow automation, and more.
---


## ✨ Features

- ✅ **Powerful Parser**: Supports arithmetic, logical, conditional, comparison operations, and custom functions.
- ✅ **Variable Binding**: Inject dynamic data using `{{variableName}}` syntax from JSON or dictionaries.
- ✅ **Extensible Functions**: Built-in functions like `IF`, `DATEDIFF`, `DATEADD`, and easy to add your own.
- ✅ **Rich Operators**: Arithmetic (`+`, `-`, `*`, `/`), logical (`&&`, `||`, `!`), comparison (`==`, `>`, `<`, `>=`, `<=`).
- ✅ **Modular Design**: Easily register `IUnaryOperator`, `IBinaryOperator`, and `IFunction` in one line.

---

## 🔧 Installation

Currently, clone the source and include it in your solution manually. NuGet package publishing is planned.

---

## 🧪 Usage Example

```csharp
using NFormula;

var parser = NFormularParser.CreateBuilder()
    .AddDefault()
    .Build();

var data = new Dictionary<string, object>
{
    ["a"] = 10,
    ["b"] = 5,
    ["flag"] = true,
    ["start"] = new DateTime(2024, 1, 1),
    ["end"] = new DateTime(2024, 1, 10),
};

var expression = parser.Parse("IF({{a}} > {{b}}, DATEDIFF({{end}}, {{start}}), 0)", data);
var result = expression.Evaluate(data); // Result: 9
```

---

## 🧩 Extend Functions/Operators

```csharp
builder.AddFunction<MyCustomFunction>();
builder.AddBinaryOperator<MyCustomOperator>();
```

Example custom function:

```csharp
public class MyCustomFunction : IFunction
{
    public string Name => "MYFUNC";
    public int MinParameterCount => 2;
    public object Evaluate(List<object> parameters)
    {
        return parameters[0].ToString() + parameters[1].ToString();
    }
}
```

---

## 🧪 Unit Testing

Includes NUnit tests for:
- Arithmetic, logic, and conditional evaluation
- Complex nested expressions
- Date/time functions
- Variable evaluation via `{{varName}}`

---

## 🏗️ Architecture Overview

- `Tokenizer`: Tokenizes expression strings
- `Parser`: Converts tokens to expression tree (Postfix)
- `Expression`: Contains constants, variables, function calls, and operators
- `Operator`: Implements logic, math, and comparison logic
- `Function`: Implements domain-specific logic
- `FormularBuilder`: Fluent API to register supported elements

---

## 🤖 Acknowledgements

This project was initiated and developed with assistance from [ChatGPT](https://openai.com/chatgpt), which supported idea shaping, expression parsing logic, and extensibility design during early prototyping.
## 👨‍💻 Author


Created by **Shikazi** (Chien Tran)

Issues and PRs are welcome!

## 📄 License

Licensed under the [MIT License](LICENSE).  
© 2025 Shikazi
