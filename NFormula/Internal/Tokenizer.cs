using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NFormula.Internal
{
    internal static class Tokenizer
    {
        private static readonly HashSet<char> OperatorChars = new HashSet<char>
        {
            '+', '-', '*', '/', '%', '^', '=', '<', '>', '!', '&', '|', '?'
        };

        public static List<Token> Tokenize(string formula,
            IEnumerable<IFunction> supportedFunctions,
            IEnumerable<IUnaryOperator> unaryOperators,
            IEnumerable<IBinaryOperator> binaryOperators)
        {
            var tokens = new List<Token>();
            var functionNames =
                new HashSet<string>(supportedFunctions.Select(f => f.Name), StringComparer.OrdinalIgnoreCase);
            var unaryOps = new HashSet<string>(unaryOperators.Select(op => op.Symbol));
            var binaryOps = new HashSet<string>(binaryOperators.Select(op => op.Symbol));

            var i = 0;
            while (i < formula.Length)
            {
                var c = formula[i];

                if (char.IsWhiteSpace(c))
                {
                    i++;
                    continue;
                }

                // Number
                if (char.IsDigit(c) || (c == '.' && i + 1 < formula.Length && char.IsDigit(formula[i + 1])))
                {
                    var start = i;
                    var hasDot = false;

                    while (i < formula.Length && (char.IsDigit(formula[i]) || formula[i] == '.'))
                    {
                        if (formula[i] == '.')
                        {
                            if (hasDot) break;
                            hasDot = true;
                        }

                        i++;
                    }

                    var number = formula.Substring(start, i - start);
                    tokens.Add(new Token(TokenType.Number, number, new LiteralValue(double.Parse(number))));
                    continue;
                }

                // String
                if (c == '"')
                {
                    i++; // skip opening "
                    var sb = new StringBuilder();
                    while (i < formula.Length && formula[i] != '"')
                    {
                        if (formula[i] == '\\' && i + 1 < formula.Length)
                        {
                            i++;
                            sb.Append(formula[i]); // naive escape
                        }
                        else
                        {
                            sb.Append(formula[i]);
                        }

                        i++;
                    }

                    if (i >= formula.Length) throw new Exception("Unterminated string literal");
                    i++; // skip closing "
                    tokens.Add(new Token(TokenType.String, sb.ToString(), new LiteralValue(sb.ToString())));
                    continue;
                }

                // Boolean
                if (formula.Substring(i).StartsWith("true", StringComparison.OrdinalIgnoreCase) &&
                    (i + 4 == formula.Length || !char.IsLetterOrDigit(formula[i + 4])))
                {
                    tokens.Add(new Token(TokenType.Boolean, "true", new LiteralValue(true)));
                    i += 4;
                    continue;
                }

                if (formula.Substring(i).StartsWith("false", StringComparison.OrdinalIgnoreCase) &&
                    (i + 5 == formula.Length || !char.IsLetterOrDigit(formula[i + 5])))
                {
                    tokens.Add(new Token(TokenType.Boolean, "false", new LiteralValue(false)));
                    i += 5;
                    continue;
                }

                // Variable {{...}}
                if (formula[i] == '{' && i + 1 < formula.Length && formula[i + 1] == '{')
                {
                    var start = i + 2;
                    var end = formula.IndexOf("}}", start, StringComparison.Ordinal);
                    if (end == -1) throw new Exception("Unterminated variable placeholder");

                    var varContent = formula.Substring(start, end - start).Trim();
                    tokens.Add(new Token(TokenType.Variable, "{{" + varContent + "}}", new LiteralValue(varContent)));
                    i = end + 2;
                    continue;
                }

                // Identifier (possible function)
                if (char.IsLetter(c) || c == '_')
                {
                    var start = i;
                    while (i < formula.Length && (char.IsLetterOrDigit(formula[i]) || formula[i] == '_')) i++;
                    var name = formula.Substring(start, i - start);

                    // Function
                    if (!functionNames.Contains(name) || i >= formula.Length || formula[i] != '(')
                        throw new Exception($"Unknown identifier: {name}");
                    tokens.Add(new Token(TokenType.Function, name));
                    continue;
                }

                // Operator
                if (OperatorChars.Contains(c))
                {
                    var op = TryReadOperator(formula, i, unaryOps, binaryOps);
                    if (op == null)
                        throw new Exception($"Unknown operator starting at {i}");

                    tokens.Add(new Token(TokenType.Operator, op));
                    i += op.Length;
                    continue;
                }

                switch (c)
                {
                    // Punctuation
                    case '(':
                        tokens.Add(new Token(TokenType.LeftParen, "(", null));
                        i++;
                        continue;
                    case ')':
                        tokens.Add(new Token(TokenType.RightParen, ")", null));
                        i++;
                        continue;
                    case ',':
                        tokens.Add(new Token(TokenType.Comma, ",", null));
                        i++;
                        continue;
                    default:
                        throw new Exception($"Unexpected character at position {i}: {c}");
                }
            }

            tokens.Add(new Token(TokenType.End, "", null));
            return tokens;
        }

        private static string TryReadOperator(string formula, int pos, HashSet<string> unaryOps,
            HashSet<string> binaryOps)
        {
            var maxLen = Math.Min(3, formula.Length - pos);
            for (var len = maxLen; len > 0; len--)
            {
                var candidate = formula.Substring(pos, len);
                if (unaryOps.Contains(candidate) || binaryOps.Contains(candidate))
                    return candidate;
            }

            return null;
        }
    }
}