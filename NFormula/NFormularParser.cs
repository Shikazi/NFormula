using System;
using System.Collections.Generic;
using System.Linq;
using NFormula.Internal;
using NFormula.Internal.Expression;

namespace NFormula
{
    public class NFormularParser : INFormularParser
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

        public IFormulaExpression Parse(string input, IVariableTypeProfiler profiler)
        {
            var tokens = Tokenizer.Tokenize(input, _functions, _unaryOps.Values.SelectMany(x => x),
                _binaryOps.Values.SelectMany(x => x));
            var postFix = ToPostfix(tokens);
            var expression = BuildExpression(postFix, profiler);
            return expression;
        }

        private List<Token> ToPostfix(List<Token> tokens)
        {
            var output = new List<Token>();
            var stack = new Stack<Token>();
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
                        break;

                    case TokenType.Comma:
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
                            output.Add(stack.Pop());
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

        private IFormulaExpression BuildExpression(List<Token> postfix, IVariableTypeProfiler typeProfiler)
        {
            var stack = new Stack<IFormulaExpression>();

            foreach (var token in postfix)
            {
                switch (token.Type)
                {
                    case TokenType.Number:
                        stack.Push(new ConstantExpression(token.Value, DataType.Number));
                        break;
                    case TokenType.String:
                        stack.Push(new ConstantExpression(token.Value, DataType.String));
                        break;
                    case TokenType.Boolean:
                        stack.Push(new ConstantExpression(token.Value, DataType.Boolean));
                        break;
                    case TokenType.Variable:
                        stack.Push(new VariableExpression(token.Text, typeProfiler.GetDataType(token.Text)));
                        break;

                    case TokenType.Function:
                    {
                        var candidates = _functions
                            .Where(f => f.Name.Equals(token.Text, StringComparison.OrdinalIgnoreCase))
                            .ToList();

                        if (candidates.Count == 0)
                            throw new Exception("Unknown function: " + token.Text);

                        foreach (var func in candidates)
                        {
                            if (stack.Count < func.ParameterTypes.Length)
                                continue; // không đủ arg để thử func này

                            // Tạm pop ra các đối số, đảo ngược đúng thứ tự
                            var args = new List<IFormulaExpression>();
                            for (int i = 0; i < func.ParameterTypes.Length; i++)
                                args.Insert(0, stack.Pop());

                            // Kiểm tra số lượng tuyệt đối và kiểu dữ liệu khớp 100%
                            bool matched = args.Count == func.ParameterTypes.Length &&
                                           args.Zip(func.ParameterTypes, (arg, expectedType) =>
                                               arg.ReturnType == expectedType
                                           ).All(x => x);

                            if (matched)
                            {
                                stack.Push(new FunctionExpression(func, args.ToArray()));
                                goto EndFunctionCase;
                            }

                            // Push lại nếu không match, để thử func khác
                            foreach (var arg in args)
                                stack.Push(arg);
                        }

                        throw new Exception($"No matching overload found for function '{token.Text}' with exact argument types.");

                        EndFunctionCase:
                        break;
                    }

                    case TokenType.Operator:
                        if (_unaryOps.TryGetValue(token.Text, out var uopList))
                        {
                            var operand = stack.Pop();
                            var op = uopList.FirstOrDefault(o => o.OperandType == operand.ReturnType);
                            if (op == null)
                                throw new Exception(
                                    $"No matching unary operator '{token.Text}' for type {operand.ReturnType}");

                            stack.Push(new UnaryOperatorExpression(operand, op));
                        }
                        else if (_binaryOps.TryGetValue(token.Text, out var bopList))
                        {
                            var right = stack.Pop();
                            var left = stack.Pop();
                            var op = bopList.FirstOrDefault(o =>
                                o.LeftType == left.ReturnType && o.RightType == right.ReturnType);
                            if (op == null)
                                throw new Exception(
                                    $"No matching binary operator '{token.Text}' for types {left.ReturnType}, {right.ReturnType}");

                            stack.Push(new BinaryOperatorExpression(left, right, op));
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