namespace NFormula
{
    public abstract class TokenMetadata {}

    public class LiteralValue : TokenMetadata
    {
        public object Value { get; }
        public LiteralValue(object value) => Value = value;
    }

    public class FunctionMetadata : TokenMetadata
    {
        public int ArgCount { get; set; }
        public FunctionMetadata(int argCount) => ArgCount = argCount;
    }

    public class Token
    {
        public TokenType Type { get; }
        public string Text { get; }
        public TokenMetadata Metadata { get; }

        public Token(TokenType type, string text, TokenMetadata metadata = null)
        {
            Type = type;
            Text = text;
            Metadata = metadata;
        }

        public T GetMetadata<T>() where T : TokenMetadata
            => Metadata as T;
    }

}