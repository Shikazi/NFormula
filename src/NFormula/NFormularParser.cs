using System;
using System.Collections.Generic;
using System.Linq;
using NFormula.Internal;
using NFormula.Internal.Expression;

namespace NFormula
{
    public class NFormularParser
    {
        private readonly List<IFunction> _functions;
        private readonly Dictionary<string, List<IUnaryOperator>> _unaryOps;
        private readonly Dictionary<string, List<IBinaryOperator>> _binaryOps;

        internal NFormularParser(
            List<IFunction> functions,
            List<IUnaryOperator> unaryOps,
            List<IBinaryOperator> binaryOps)
        {
            _functions = functions;

            _unaryOps = new Dictionary<string, List<IUnaryOperator>>(StringComparer.OrdinalIgnoreCase);
            foreach (var uop in unaryOps)
            {
                if (!_unaryOps.TryGetValue(uop.Symbol, out var list))
                    _unaryOps[uop.Symbol] = list = new List<IUnaryOperator>();
                list.Add(uop);
            }

            _binaryOps = new Dictionary<string, List<IBinaryOperator>>(StringComparer.OrdinalIgnoreCase);
            foreach (var bop in binaryOps)
            {
                if (!_binaryOps.TryGetValue(bop.Symbol, out var list))
                    _binaryOps[bop.Symbol] = list = new List<IBinaryOperator>();
                list.Add(bop);
            }
        }

        public static FormularBuilder CreateBuilder()
        {
            return new FormularBuilder();
        }

        public IFormulaExpression Parse(string input)
        {
            var tokens = Tokenizer.Tokenize(input, _functions, _unaryOps.Values.SelectMany(x => x),
                _binaryOps.Values.SelectMany(x => x));
            var postFix = ToPostfix(tokens);
            var expression = BuildExpression(postFix);
            return expression;
        }

        private List<Token> ToPostfix(List<Token> tokens)
        {
            var output = new List<Token>();
            var stack = new Stack<Token>();
            var functionArgCount = new Stack<int>();
            Token prevToken = null;

            foreach (var token in tokens)
            {
                switch (token.Type)
                {
                    case TokenType.Number:
                    case TokenType.String:
                    case TokenType.Boolean:
                    case TokenType.Variable:
                        output.Add(token);
                        break;

                    case TokenType.Function:
                        stack.Push(token);
                        functionArgCount.Push(0);
                        break;

                    case TokenType.Comma:
                        if (functionArgCount.Count > 0)
                            functionArgCount.Push(functionArgCount.Pop() + 1);

                        while (stack.Count > 0 && stack.Peek().Type != TokenType.LeftParen)
                            output.Add(stack.Pop());
                        break;

                    case TokenType.Operator:
                        var isUnary = IsUnaryOperator(token, prevToken);
                        while (stack.Count > 0 && stack.Peek().Type == TokenType.Operator)
                        {
                            var op2 = stack.Peek();
                            var currPrec = GetPrecedence(token.Text, isUnary);
                            var stackPrec = GetPrecedence(op2.Text, IsUnaryOperator(op2, null));

                            if ((IsRightAssociative(token.Text, isUnary) && currPrec < stackPrec) ||
                                (!IsRightAssociative(token.Text, isUnary) && currPrec <= stackPrec))
                                output.Add(stack.Pop());
                            else
                                break;
                        }

                        stack.Push(token);
                        break;

                    case TokenType.LeftParen:
                        stack.Push(token);
                        break;

                    case TokenType.RightParen:
                        while (stack.Count > 0 && stack.Peek().Type != TokenType.LeftParen)
                            output.Add(stack.Pop());
                        if (stack.Count == 0)
                            throw new Exception("Mismatched parentheses");
                        stack.Pop();
                        if (stack.Count > 0 && stack.Peek().Type == TokenType.Function)
                        {
                            var func = stack.Pop();
                            var argCount = functionArgCount.Pop();
                            if (prevToken?.Type != TokenType.LeftParen)
                                argCount++;
                            func.SetMetadata(new FunctionMetadata(argCount));

                            output.Add(func);
                        }

                        break;

                    case TokenType.End:
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }

                prevToken = token.Type == TokenType.End ? null : token;
            }

            while (stack.Count > 0)
            {
                var t = stack.Pop();
                if (t.Type == TokenType.LeftParen || t.Type == TokenType.RightParen)
                    throw new Exception("Mismatched parentheses");
                output.Add(t);
            }

            return output;
        }

        private IFormulaExpression BuildExpression(List<Token> postfix)
        {
            var stack = new Stack<IFormulaExpression>();

            foreach (var token in postfix)
            {
                switch (token.Type)
                {
                    case TokenType.Number:
                        stack.Push(new ConstantExpression(token.GetMetadata<LiteralValue>().Value, DataType.Number));
                        break;
                    case TokenType.String:
                        stack.Push(new ConstantExpression(token.GetMetadata<LiteralValue>().Value, DataType.String));
                        break;
                    case TokenType.Boolean:
                        stack.Push(new ConstantExpression(token.GetMetadata<LiteralValue>().Value, DataType.Boolean));
                        break;
                    case TokenType.Variable:
                        var name = token.GetMetadata<LiteralValue>().Value.ToString();
                        stack.Push(new VariableExpression(name));
                        break;

                    case TokenType.Function:
                    {
                        var candidates = _functions
                            .Where(f => f.Name.Equals(token.Text, StringComparison.OrdinalIgnoreCase))
                            .ToList();
                        if (candidates.Count == 0)
                            throw new Exception("Unknown function: " + token.Text);
                        var args = new List<IFormulaExpression>();
                        var argCount = token.GetMetadata<FunctionMetadata>().ArgCount;
                        for (var i = 0; i < argCount; i++)
                            args.Insert(0, stack.Pop());
                        stack.Push(new FunctionExpression(candidates, args.ToArray()));
                        break;
                    }

                    case TokenType.Operator:
                        if (_unaryOps.TryGetValue(token.Text, out var uopList))
                        {
                            var operand = stack.Pop();
                            if (uopList == null)
                                throw new Exception(
                                    $"No matching unary operator '{token.Text}'");

                            stack.Push(new UnaryOperatorExpression(operand, uopList));
                        }
                        else if (_binaryOps.TryGetValue(token.Text, out var bopList))
                        {
                            var right = stack.Pop();
                            var left = stack.Pop();

                            if (bopList == null)
                                throw new Exception(
                                    $"No matching binary operator '{token.Text}' ");

                            stack.Push(new BinaryOperatorExpression(left, right, bopList));
                        }
                        else
                        {
                            throw new Exception("Unknown operator: " + token.Text);
                        }

                        break;

                    default:
                        throw new InvalidOperationException("Unsupported token in expression: " + token);
                }
            }

            if (stack.Count != 1)
                throw new Exception("Invalid expression. Remaining stack: " + stack.Count);

            return stack.Pop();
        }

        private bool IsUnaryOperator(Token token, Token prev)
        {
            return (prev == null ||
                    prev.Type == TokenType.Operator ||
                    prev.Type == TokenType.LeftParen ||
                    prev.Type == TokenType.Comma) &&
                   _unaryOps.ContainsKey(token.Text);
        }

        private int GetPrecedence(string symbol, bool isUnary)
        {
            switch (isUnary)
            {
                case true when _unaryOps.TryGetValue(symbol, out var uops) && uops.Count > 0:
                    return uops[0].Precedence;
                case false when _binaryOps.TryGetValue(symbol, out var bops) && bops.Count > 0:
                    return bops[0].Precedence;
                default:
                    return 0;
            }
        }

        private bool IsRightAssociative(string symbol, bool isUnary)
        {
            switch (isUnary)
            {
                case true when _unaryOps.TryGetValue(symbol, out var uops) && uops.Count > 0:
                    return uops[0].IsRightAssociative;
                case false when _binaryOps.TryGetValue(symbol, out var bops) && bops.Count > 0:
                    return bops[0].IsRightAssociative;
                default:
                    return false;
            }
        }
    }
}