using System;

namespace NFormula.Default.Operator.Comparison
{
    public class NumberLessThanOrEqualOperator : IBinaryOperator
    {
        public DataType LeftType => DataType.Number;
        public DataType RightType => DataType.Number;
        public DataType ReturnType => DataType.Boolean;

        public string Symbol => "<=";
        public int Precedence => 2;
        public bool IsRightAssociative => false;

        public object Evaluate(object left, object right)
        {
            return Convert.ToDouble(left) <= Convert.ToDouble(right);
        }
    }
    public class DateTimeLessThanOrEqualOperator : IBinaryOperator
    {
        public DataType LeftType => DataType.DateTime;
        public DataType RightType  => DataType.DateTime;
        public DataType ReturnType  => DataType.Boolean;
        public object Evaluate(object left, object right)
        {
            return Convert.ToDateTime(left) <= Convert.ToDateTime(right);
        }

        public string Symbol => "<=";
        public int Precedence => 2;
        public bool IsRightAssociative => false;
    }
}