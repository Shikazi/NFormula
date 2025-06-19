namespace NFormula.Internal.Expression
{
    /// <summary>
    /// Represents a unary operator expression (e.g. -x, !flag).
    /// </summary>
    internal sealed class UnaryOperatorExpression : IFormulaExpression
    {
        private readonly IFormulaExpression _operand;
        private readonly IUnaryOperator _unaryOperator;

        public UnaryOperatorExpression(IFormulaExpression operand, IUnaryOperator op)
        {
            _operand = operand;
            _unaryOperator = op;
        }

        public DataType ReturnType => _unaryOperator.ReturnType;

        public object Evaluate(IEvaluationContext context)
        {
            var value = _operand.Evaluate(context);
            return _unaryOperator.Evaluate(value);
        }
    }
}