using System.Collections.Generic;

namespace NFormula
{
    public interface IFunction
    {
        DataType[] ParameterTypes { get; }
        DataType ReturnType { get; }
        object Evaluate(IList<object> args);
        string Name { get; }
    }
}
