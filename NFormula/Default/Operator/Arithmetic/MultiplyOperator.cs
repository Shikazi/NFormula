using System;

namespace NFormula.Default.Operator.Arithmetic
{
    public class MultiplyOperator : IBinaryOperator
    {
        public DataType LeftType => DataType.Number;
        public DataType RightType => DataType.Number;
        public DataType ReturnType => DataType.Number;

        public string Symbol => "*";
        public int Precedence => 2;
        public bool IsRightAssociative => false;

        public object Evaluate(object left, object right)
        {
            if (left == null || right == null)
                throw new ArgumentNullException("Operands for * cannot be null.");

            return Convert.ToDouble(left) * Convert.ToDouble(right);
        }
    }
}