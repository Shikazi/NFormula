using NFormula;

namespace NFormular.Tests;

public class VariableContext(Dictionary<string, object> variables) : IVariableTypeProfiler, IEvaluationContext
{
    public object GetVariableValue(string name)
    {
        return variables.GetValueOrDefault(name);
    }

    public bool HasVariable(string name)
    {
        return variables.ContainsKey(name);
    }

    DataType IEvaluationContext.GetDataType(string name)
    {
        return GetDataTypeInternal(name);
    }

    DataType IVariableTypeProfiler.GetDataType(string name)
    {
        return GetDataTypeInternal(name);
    }

    private DataType GetDataTypeInternal(string name)
    {
        if (!variables.TryGetValue(name, out var value))
        {
            throw new KeyNotFoundException($"Variable '{name}' not found or null.");
        }

        return value switch
        {
            string => DataType.String,
            int or long or float or double or decimal => DataType.Number,
            bool => DataType.Boolean,
            DateTime => DataType.DateTime,
            IEnumerable<object> => DataType.Array,
            _ => throw new NotSupportedException($"Unsupported variable type for '{name}': {value.GetType().Name}")
        };
    }
}