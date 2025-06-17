namespace NFormula
{
    public interface IUnaryOperator
    {
        string Symbol { get; }
        int Precedence { get; }
        bool IsRightAssociative { get; } 

        DataType OperandType { get; }
        DataType ReturnType { get; }

        object Evaluate(object operand);
    }
}