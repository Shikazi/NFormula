namespace NFormula
{
    public enum TokenType
    {
        Number,         // 123, 45.6
        String,         // "abc", 'xyz'
        Boolean,        // true, false
        Variable,       // $varName
        Function,       // FUNC(...
        Operator,       // +, -, *, /, etc.
        Comma,          // ,
        LeftParen,      // (
        RightParen,     // )
        End             // EOF marker
    }

}
