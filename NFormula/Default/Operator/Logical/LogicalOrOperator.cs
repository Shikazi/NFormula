using System;

namespace NFormula.Default.Operator.Logical
{
    public class LogicalOrOperator : IBinaryOperator
    {
        public DataType LeftType => DataType.Boolean;
        public DataType RightType => DataType.Boolean;
        public DataType ReturnType => DataType.Boolean;

        public string Symbol => "||";
        public int Precedence => 4;
        public bool IsRightAssociative => false;

        public object Evaluate(object left, object right)
        {
            if (left == null || right == null)
                return false;

            return Convert.ToBoolean(left) || Convert.ToBoolean(right);
        }
    }
}