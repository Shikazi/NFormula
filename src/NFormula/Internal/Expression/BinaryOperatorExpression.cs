namespace NFormula.Internal.Expression
{
    /// <summary>
    /// Represents a binary operator expression (e.g. a + b).
    /// </summary>
    internal sealed class BinaryOperatorExpression : IFormulaExpression
    {
        private readonly IFormulaExpression _left;
        private readonly IFormulaExpression _right;
        private readonly IBinaryOperator _binaryOperator;

        public BinaryOperatorExpression(IFormulaExpression left, IFormulaExpression right, IBinaryOperator op)
        {
            _left = left;
            _right = right;
            _binaryOperator = op;
        }

        public DataType ReturnType => _binaryOperator.ReturnType;

        public object Evaluate(IEvaluationContext context)
        {
            var leftValue = _left.Evaluate(context);
            var rightValue = _right.Evaluate(context);
            return _binaryOperator.Evaluate(leftValue, rightValue);
        }
    }
}