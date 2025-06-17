namespace NFormula.Internal.Expression
{
    /// <summary>
    /// Represents a reference to a variable (e.g., $x).
    /// </summary>
    internal sealed class VariableExpression : IFormulaExpression
    {
        public VariableExpression(string name, DataType returnType)
        {
            Name = name;
            ReturnType = returnType;
        }

        private string Name { get; }

        public DataType ReturnType { get; }

        public object Evaluate(IEvaluationContext context)
        {
            return context.GetVariableValue(Name);
        }
    }
}