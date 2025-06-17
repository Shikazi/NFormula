namespace NFormula
{
    public interface IBinaryOperator
    {
        DataType LeftType { get; }
        DataType RightType { get; }
        DataType ReturnType { get; }
        object Evaluate(object left, object right);
        string Symbol { get; }
        int Precedence { get; }
        bool IsRightAssociative { get; }
    }
}
