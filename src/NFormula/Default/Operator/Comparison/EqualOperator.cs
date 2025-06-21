using System;

namespace NFormula.Default.Operator.Comparison
{
    public class BooleanEqualOperator : IBinaryOperator
    {
        public DataType LeftType => DataType.Boolean;
        public DataType RightType => DataType.Boolean;
        public DataType ReturnType => DataType.Boolean;

        public string Symbol => "==";
        public int Precedence => 0;
        public bool IsRightAssociative => false;

        public object Evaluate(object left, object right)
        {
            if (left == null || right == null)
                return false;

            return Convert.ToBoolean(left) == Convert.ToBoolean(right);
        }
    }

    public class NumberEqualOperator : IBinaryOperator
    {
        public DataType LeftType => DataType.Number;
        public DataType RightType => DataType.Number;
        public DataType ReturnType => DataType.Boolean;

        public string Symbol => "==";
        public int Precedence => 0;
        public bool IsRightAssociative => false;

        public object Evaluate(object left, object right)
        {
            if (left == null && right == null)
                return true;

            if (left == null || right == null)
                return false;

            return left.Equals(right);
        }
    }

    public class StringEqualOperator : IBinaryOperator
    {
        public DataType LeftType => DataType.String;
        public DataType RightType => DataType.String;
        public DataType ReturnType => DataType.Boolean;

        public string Symbol => "==";
        public int Precedence => 0;
        public bool IsRightAssociative => false;

        public object Evaluate(object left, object right)
        {
            if (left == null && right == null)
                return true;

            if (left == null || right == null)
                return false;

            return left.Equals(right);
        }
    }

    public class DateTimeEqualOperator : IBinaryOperator
    {
        public DataType LeftType => DataType.DateTime;
        public DataType RightType  => DataType.DateTime;
        public DataType ReturnType  => DataType.Boolean;
        public object Evaluate(object left, object right)
        {
            if (left == null && right == null)
                return true;

            if (left == null || right == null)
                return false;

            return left.Equals(right);
        }

        public string Symbol => "==";
        public int Precedence => 0;
        public bool IsRightAssociative => false;
    }
}