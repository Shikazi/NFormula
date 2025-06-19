using System;
using System.Collections.Generic;

namespace NFormula.Default.Functions
{
    public class DateDiff : IFunction
    {
        public DataType[] ParameterTypes { get; } = { DataType.DateTime, DataType.DateTime };
        public DataType ReturnType => DataType.Number;

        public object Evaluate(IList<object> args)
        {
            if (args == null || args.Count != 2)
                throw new ArgumentException("DATEDIFF requires exactly 2 arguments.");

            if (!DateTime.TryParse(args[0]?.ToString(), out var date1))
                throw new ArgumentException("First argument is not a valid date.");

            if (!DateTime.TryParse(args[1]?.ToString(), out var date2))
                throw new ArgumentException("Second argument is not a valid date.");

            return (date1 - date2).TotalDays;
        }

        public string Name => "DATEDIFF";
    }
}