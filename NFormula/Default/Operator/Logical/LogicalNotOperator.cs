using System;

namespace NFormula.Default.Operator.Logical
{
    public class LogicalNotOperator : IUnaryOperator
    {
        public DataType OperandType => DataType.Boolean;
        public DataType ReturnType => DataType.Boolean;

        public string Symbol => "!";
        public int Precedence => 4;
        public bool IsRightAssociative => true;

        public object Evaluate(object operand)
        {
            switch (operand)
            {
                case null:
                    throw new ArgumentNullException("operand");
                case bool booleanValue:
                    return !booleanValue;
                default:
                    throw new InvalidCastException("Operand for ! must be a boolean.");
            }
        }
    }
}