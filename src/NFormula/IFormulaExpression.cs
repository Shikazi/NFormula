namespace NFormula
{
    /// <summary>
    /// Represents a node in the abstract syntax tree of a formula.
    /// </summary>
    public interface IFormulaExpression
    {
        /// <summary>
        /// Evaluates the expression using the provided context.
        /// </summary>
        /// <param name="context">The evaluation context which provides variable/function resolution.</param>
        /// <returns>The result of the evaluation.</returns>
        object Evaluate(IEvaluationContext context);

        DataType GetReturnType(IDataTypeContext context);
    }
}