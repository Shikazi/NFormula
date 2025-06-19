using System;

namespace NFormula.Default.Operator.Arithmetic
{
    public class PowerOperator : IBinaryOperator
    {
        public DataType LeftType => DataType.Number;
        public DataType RightType => DataType.Number;
        public DataType ReturnType => DataType.Number;

        public string Symbol => "^";
        public int Precedence => 3;
        public bool IsRightAssociative => true;

        public object Evaluate(object left, object right)
        {
            if (left == null || right == null)
                throw new ArgumentNullException("Operands for ^ cannot be null.");

            return Math.Pow(Convert.ToDouble(left), Convert.ToDouble(right));
        }
    }
}