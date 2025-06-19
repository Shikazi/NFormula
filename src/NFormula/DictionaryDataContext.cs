using System;
using System.Collections;
using System.Collections.Generic;

namespace NFormula
{
    public class DictionaryDataContext
        : IVariableTypeProfiler, IEvaluationContext
    {
        private readonly Dictionary<string, object> _data;

        public DictionaryDataContext(Dictionary<string, object> data)
        {
            _data = data;
        }

        public object GetVariableValue(string name)
        {
            if (!_data.TryGetValue(name, out var value))
            {
                throw new KeyNotFoundException($"Variable '{name}' not found.");
            }

            return value;
        }

        public bool HasVariable(string name)
        {
            return _data.ContainsKey(name);
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
            if (!_data.TryGetValue(name, out var value) || value == null)
            {
                throw new KeyNotFoundException($"Variable '{name}' not found or null.");
            }

            switch (value)
            {
                case string _:
                    return DataType.String;
                case int _:
                case long _:
                case float _:
                case double _:
                case decimal _:
                    return DataType.Number;
                case bool _:
                    return DataType.Boolean;
                case DateTime _:
                    return DataType.DateTime;
                case IEnumerable _:
                    return DataType.Array;
                default:
                    throw new NotSupportedException($"Unsupported variable type for '{name}': {value.GetType().Name}");
            }
        }
    }
}