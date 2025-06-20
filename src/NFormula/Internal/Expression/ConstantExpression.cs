namespace NFormula.Internal.Expression
{
    /// <summary>
    /// Represents a constant value in the formula.
    /// </summary>
    internal sealed class ConstantExpression : IFormulaExpression
    {
        private readonly object _value;
        private DataType ReturnType { get; }

        public ConstantExpression(object value, DataType type)
        {
            _value = value;
            ReturnType = type;
        }


        public object Evaluate(IEvaluationContext context)
        {
            return _value;
        }

        public DataType GetReturnType(IDataTypeContext context)
        {
            return ReturnType;
        }
    }
}