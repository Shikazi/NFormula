using System.Runtime.InteropServices.JavaScript;
using NFormula;
using NFormula.Default.Functions;
using NFormula.Default.Operator.Arithmetic;
using NFormula.Default.Operator.Comparison;
using NFormula.Default.Operator.Logical;
using NFormula.Internal;
using NFormula.Internal.Expression;
using NUnit.Framework.Legacy;

namespace NFormular.Tests;

public class TokenizerTests
{
    private readonly List<IUnaryOperator> _unaryOperators =
    [
        new LogicalNotOperator()
    ];

    private readonly List<IBinaryOperator> _binaryOperators =
    [
        new LogicalAndOperator(),
        new LogicalOrOperator(),
        new AddOperator(),
        new SubtractOperator(),
        new MultiplyOperator(),
        new DivideOperator(),
        new StringEqualOperator(),
        new NumberEqualOperator(),
        new BooleanEqualOperator(),
        new NumberGreaterThanOperator(),
        new NumberGreaterThanOrEqualOperator(),
        new NumberLessThanOperator(),
        new NumberLessThanOrEqualOperator(),
        new DateTimeEqualOperator(),
        new DatetimeGreaterThanOperator(),
        new DateTimeGreaterThanOrEqualOperator(),
        new DateTimeLessThanOperator(),
        new DateTimeLessThanOrEqualOperator(),
    ];

    private readonly List<IFunction> _functions =
    [
        new IfBooleanFunction(),
        new IfArrayTimeFunction(),
        new IfDateTimeFunction(),
        new IfNumberFunction(),
        new IfStringFunction(),
        new DateDiff(),
        new DateAddFunction()
    ];

    private List<Token> GetToken(string token)
    {
        return Tokenizer.Tokenize(token, _functions, _unaryOperators, _binaryOperators);
    }

    [Test]
    public void Tokenize_SimpleArithmetic_ShouldGenerateCorrectTokens()
    {
        var tokens = GetToken("1 + 2");


        Assert.Multiple(() =>
        {
            Assert.That(tokens, Has.Count.EqualTo(4));
            Assert.That(tokens[0].Type, Is.EqualTo(TokenType.Number));
            Assert.That(tokens[0].Text, Is.EqualTo("1"));
            Assert.That(tokens[1].Type, Is.EqualTo(TokenType.Operator));
            Assert.That(tokens[1].Text, Is.EqualTo("+"));
            Assert.That(tokens[2].Type, Is.EqualTo(TokenType.Number));
            Assert.That(tokens[2].Text, Is.EqualTo("2"));
            Assert.That(tokens.Last().Type, Is.EqualTo(TokenType.End));
        });
    }

    [Test]
    public void Tokenize_WithVariable_ShouldParseCurlyBraces()
    {
        var tokens = GetToken("{{abc}} + 5");

        Assert.Multiple(() =>
        {
            Assert.That(tokens, Has.Count.EqualTo(4));
            Assert.That(tokens[0].Type, Is.EqualTo(TokenType.Variable));
            Assert.That(tokens[0].Text, Is.EqualTo("{{abc}}"));

            Assert.That(tokens[1].Type, Is.EqualTo(TokenType.Operator));
            Assert.That(tokens[1].Text, Is.EqualTo("+"));

            Assert.That(tokens[2].Type, Is.EqualTo(TokenType.Number));
            Assert.That(tokens[2].Text, Is.EqualTo("5"));

            Assert.That(tokens[3].Type, Is.EqualTo(TokenType.End));
        });
    }


    [Test]
    public void Tokenize_IfFunction_ShouldReturnCorrectTokens()
    {
        var tokens = GetToken("IF(true, 1, 2)");

        Assert.Multiple(() =>
        {
            Assert.That(tokens, Has.Count.EqualTo(9));

            Assert.That(tokens[0].Type, Is.EqualTo(TokenType.Function));
            Assert.That(tokens[0].Text, Is.EqualTo("IF"));

            Assert.That(tokens[1].Type, Is.EqualTo(TokenType.LeftParen));
            Assert.That(tokens[1].Text, Is.EqualTo("("));

            Assert.That(tokens[2].Type, Is.EqualTo(TokenType.Boolean));
            Assert.That(tokens[2].Text, Is.EqualTo("true"));

            Assert.That(tokens[3].Type, Is.EqualTo(TokenType.Comma));
            Assert.That(tokens[3].Text, Is.EqualTo(","));

            Assert.That(tokens[4].Type, Is.EqualTo(TokenType.Number));
            Assert.That(tokens[4].Text, Is.EqualTo("1"));

            Assert.That(tokens[5].Type, Is.EqualTo(TokenType.Comma));
            Assert.That(tokens[5].Text, Is.EqualTo(","));

            Assert.That(tokens[6].Type, Is.EqualTo(TokenType.Number));
            Assert.That(tokens[6].Text, Is.EqualTo("2"));

            Assert.That(tokens[7].Type, Is.EqualTo(TokenType.RightParen));
            Assert.That(tokens[7].Text, Is.EqualTo(")"));

            Assert.That(tokens[8].Type, Is.EqualTo(TokenType.End));
        });
    }
}