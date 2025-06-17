namespace NFormula.Internal.Expression
{
    /// <summary>
    /// Represents a constant value in the formula.
    /// </summary>
    internal sealed class ConstantExpression : IFormulaExpression
    {
        private readonly object _value;

        public ConstantExpression(object value, DataType type)
        {
            _value = value;
            ReturnType = type;
        }

        public DataType ReturnType { get; }

        public object Evaluate(IEvaluationContext context)
        {
            return _value;
        }
    }
}