using System;

namespace NFormula.Default.Operator.Arithmetic
{
    public class DivideOperator : IBinaryOperator
    {
        public DataType LeftType => DataType.Number;
        public DataType RightType => DataType.Number;
        public DataType ReturnType => DataType.Number;

        public string Symbol => "/";
        public int Precedence => 2;
        public bool IsRightAssociative => false;

        public object Evaluate(object left, object right)
        {
            if (left == null || right == null)
                throw new ArgumentNullException("Operands for / cannot be null.");

            var denominator = Convert.ToDouble(right);
            if (denominator == 0)
                throw new DivideByZeroException("Division by zero.");

            return Convert.ToDouble(left) / denominator;
        }
    }
}