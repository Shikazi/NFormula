namespace NFormula
{
    public class Token
    {
        public Token(TokenType type, string text, object value)
        {
            this.Type = type;
            this.Text = text;
            this.Value = value;
        }
        public TokenType Type { get; set; }
        public string Text { get; set; } = string.Empty;
        public object Value { get; set; }

        public override string ToString() => $"{Type}: {Text}";
    }
    
}
