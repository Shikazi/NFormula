using NFormula;

namespace NFormular.Tests;

[TestFixture]
public class FormulaTests
{
    private DictionaryDataContext _dataContext;

    [SetUp]
    public void Setup()
    {
        var data = new Dictionary<string, object>
        {
            ["number1"] = 10,
            ["number2"] = 5,
            ["text1"] = "abc",
            ["text2"] = "abc",
            ["boolTrue"] = true,
            ["boolFalse"] = false,
            ["date1"] = new DateTime(2024, 1, 1),
            ["date2"] = new DateTime(2024, 1, 10),
        };
        _dataContext = new DictionaryDataContext(data);
    }

    private NFormularParser CreateParser()
    {
        return NFormularParser.CreateBuilder()
            .AddDefault()
            .Build();
    }

    [Test]
    public void Test_Addition_With_Variables()
    {
        var parser = CreateParser();
        var expr = parser.Parse("{{number1}} + {{number2}}", _dataContext);
        var result = expr.Evaluate(_dataContext);
        Assert.That(result, Is.EqualTo(15));
    }

    [Test]
    public void Test_Logical_And()
    {
        var parser = CreateParser();
        var expr = parser.Parse("{{boolTrue}} && {{boolFalse}}", _dataContext);
        var result = expr.Evaluate(_dataContext);
        Assert.That(result, Is.False);
    }

    [Test]
    public void Test_Equal_String()
    {
        var parser = CreateParser();
        var expr = parser.Parse("{{text1}} == {{text2}}", _dataContext);
        var result = expr.Evaluate(_dataContext);
        Assert.That(result, Is.True);
    }

    [Test]
    public void Test_Comparison_Greater_Than()
    {
        var parser = CreateParser();
        var expr = parser.Parse("{{number1}} > {{number2}}", _dataContext);
        var result = expr.Evaluate(_dataContext);
        Assert.That(result, Is.True);
    }

    [Test]
    public void Test_Nested_If_Function()
    {
        var parser = CreateParser();
        var expr = parser.Parse("IF({{boolTrue}}, 100, 200)", _dataContext);
        var result = expr.Evaluate(_dataContext);
        Assert.That(result, Is.EqualTo(100));
    }

    [Test]
    public void Test_DateDiff_Function()
    {
        var parser = CreateParser();
        var expr = parser.Parse("DATEDIFF({{date2}}, {{date1}})", _dataContext);
        var result = expr.Evaluate(_dataContext);
        Assert.That(result, Is.EqualTo(9));
    }

    [Test]
    public void Test_Complex_Expression_Mix()
    {
        var parser = CreateParser();
        var expr = parser.Parse("IF({{number1}} > {{number2}}, {{number1}} + 5, {{number2}} - 2)", _dataContext);
        var result = expr.Evaluate(_dataContext);
        Assert.That(result, Is.EqualTo(15));
    }

    [Test]
    public void Test_Not_Operator()
    {
        var parser = CreateParser();
        var expr = parser.Parse("!{{boolFalse}}", _dataContext);
        var result = expr.Evaluate(_dataContext);
        Assert.That(result, Is.True);
    }
}