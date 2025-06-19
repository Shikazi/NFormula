using System;

namespace NFormula.Default.Operator.Arithmetic
{
    public class AddOperator : IBinaryOperator
    {
        public DataType LeftType => DataType.Number;
        public DataType RightType => DataType.Number;
        public DataType ReturnType => DataType.Number;

        public string Symbol => "+";
        public int Precedence => 1;
        public bool IsRightAssociative => false;

        public object Evaluate(object left, object right)
        {
            if (left == null || right == null)
                throw new ArgumentNullException("Operands for + cannot be null.");

            try
            {
                var leftVal = Convert.ToDouble(left);
                var rightVal = Convert.ToDouble(right);
                return leftVal + rightVal;
            }
            catch (FormatException)
            {
                throw new InvalidCastException("Operands for + must be convertible to double.");
            }
        }
    }
}