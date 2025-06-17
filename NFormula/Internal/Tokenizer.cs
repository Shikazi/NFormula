using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace NFormula.Internal
{
    internal sealed class Tokenizer
    {
        private static readonly Regex NumberRegex = new Regex(@"^\d+(\.\d+)?", RegexOptions.Compiled);

        private static readonly Regex StringRegex =
            new Regex("^\"([^\\\\\"]|\\\\.)*\"|^'([^\\\\']|\\\\.)*'", RegexOptions.Compiled);

        private static readonly Regex VariableRegex = new Regex(@"^\$[a-zA-Z_][a-zA-Z0-9_\\.]*", RegexOptions.Compiled);
        private static readonly Regex IdentifierRegex = new Regex(@"^[a-zA-Z_][a-zA-Z0-9_]*", RegexOptions.Compiled);
        private static readonly Regex OperatorRegex = new Regex(@"^([+\-*/%=!<>&|?:.^#]{1,3})", RegexOptions.Compiled);

        private Tokenizer()
        {
        }

        public static List<Token> Tokenize(
            string input,
            IEnumerable<IFunction> functions,
            IEnumerable<IUnaryOperator> unaryOperators,
            IEnumerable<IBinaryOperator> binaryOperators)
        {
            var tokens = new List<Token>();
            var pos = 0;
            var length = input.Length;

            while (pos < length)
            {
                var ch = input[pos];

                // Skip whitespace
                if (char.IsWhiteSpace(ch))
                {
                    pos++;
                    continue;
                }

                var remaining = input.Substring(pos);

                // Number
                var match =
                    NumberRegex.Match(remaining);
                if (match.Success)
                {
                    var number = match.Value;
                    tokens.Add(new Token(TokenType.Number, number, double.Parse(number)));
                    pos += number.Length;
                    continue;
                }

                // String
                match = StringRegex.Match(remaining);
                if (match.Success)
                {
                    var str = match.Value;
                    var val = str.Substring(1, str.Length - 2).Replace("\\\"", "\"").Replace("\\'", "'");
                    tokens.Add(new Token(TokenType.String, str, val));
                    pos += str.Length;
                    continue;
                }

                // Boolean
                if (remaining.StartsWith("true", StringComparison.OrdinalIgnoreCase) &&
                    (remaining.Length == 4 || !char.IsLetterOrDigit(remaining[4])))
                {
                    tokens.Add(new Token(TokenType.Boolean, "true", true));
                    pos += 4;
                    continue;
                }

                if (remaining.StartsWith("false", StringComparison.OrdinalIgnoreCase) &&
                    (remaining.Length == 5 || !char.IsLetterOrDigit(remaining[5])))
                {
                    tokens.Add(new Token(TokenType.Boolean, "false", false));
                    pos += 5;
                    continue;
                }

                // Variable
                match = VariableRegex.Match(remaining);
                if (match.Success)
                {
                    tokens.Add(new Token(TokenType.Variable, match.Value, null));
                    pos += match.Length;
                    continue;
                }

                // Function
                match = IdentifierRegex.Match(remaining);
                if (match.Success)
                {
                    var name = match.Value;
                    if (IsFunction(name, functions) &&
                        pos + name.Length < length &&
                        input[pos + name.Length] == '(')
                    {
                        tokens.Add(new Token(TokenType.Function, name, null));
                        pos += name.Length;
                        continue;
                    }
                }

                // Operator
                match = OperatorRegex.Match(remaining);
                if (match.Success && IsOperator(match.Value, unaryOperators, binaryOperators))
                {
                    tokens.Add(new Token(TokenType.Operator, match.Value, match.Value));
                    pos += match.Length;
                    continue;
                }

                // Punctuation
                switch (ch)
                {
                    case '(':
                        tokens.Add(new Token(TokenType.LeftParen, "(", null));
                        pos++;
                        continue;
                    case ')':
                        tokens.Add(new Token(TokenType.RightParen, ")", null));
                        pos++;
                        continue;
                    case ',':
                        tokens.Add(new Token(TokenType.Comma, ",", null));
                        pos++;
                        continue;
                }

                // Unknown
                throw new InvalidOperationException("Unrecognized token at position " + pos + ": " + remaining);
            }

            tokens.Add(new Token(TokenType.End, "", null));
            return tokens;
        }

        private static bool IsFunction(string name, IEnumerable<IFunction> functions)
        {
            var upper = name.ToUpperInvariant();
            return functions.Any(f => f.Name.ToUpperInvariant() == upper);
        }

        private static bool IsOperator(string symbol, IEnumerable<IUnaryOperator> unaryOps, IEnumerable<IBinaryOperator> binaryOps)
        {
            return unaryOps.Any(op => op.Symbol == symbol) || binaryOps.Any(op => op.Symbol == symbol);
        }
    }
}