namespace NFormula.Internal.Expression
{
    /// <summary>
    /// Represents a reference to a variable (e.g., $x).
    /// </summary>
    internal sealed class VariableExpression : IFormulaExpression
    {
        public VariableExpression(string name)
        {
            Name = name;
        }

        private string Name { get; }

        public object Evaluate(IEvaluationContext context)
        {
            return context.GetVariableValue(Name);
        }

        public DataType GetReturnType(IDataTypeContext context)
        {
            return context.GetDataType(Name);
        }
    }
}