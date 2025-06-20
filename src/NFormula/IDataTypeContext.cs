namespace NFormula
{
    public interface IDataTypeContext
    {
        /// <summary>
        /// Determines whether a variable with the specified name exists in the context.
        /// </summary>
        /// <param name="name">The name of the variable.</param>
        /// <returns>True if the variable exists; otherwise, false.</returns>
        bool HasVariable(string name);

        DataType GetDataType(string name);
    }
}