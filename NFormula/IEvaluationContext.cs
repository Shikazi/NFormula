namespace NFormula
{
    /// <summary>
    /// Provides access to variable values during formula evaluation.
    /// This interface should be implemented by any context that supplies data to formula expressions.
    /// </summary>
    public interface IEvaluationContext
    {
        /// <summary>
        /// Gets the value of a variable by its name.
        /// If the variable is not found, implementors may return null or throw an exception.
        /// </summary>
        /// <param name="name">The name of the variable (without leading '$').</param>
        /// <returns>The value of the variable.</returns>
        object GetVariableValue(string name);

        /// <summary>
        /// Determines whether a variable with the specified name exists in the context.
        /// </summary>
        /// <param name="name">The name of the variable.</param>
        /// <returns>True if the variable exists; otherwise, false.</returns>
        bool HasVariable(string name);

        DataType GetDataType(string name);
    }
}