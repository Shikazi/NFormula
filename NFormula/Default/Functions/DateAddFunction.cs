using System;
using System.Collections.Generic;

namespace NFormula.Default.Functions
{
    public class DateAddFunction : IFunction
    {
        public DataType[] ParameterTypes { get; } =
            { DataType.String, DataType.Number, DataType.String };

        public DataType ReturnType => DataType.String;

        public object Evaluate(IList<object> args)
        {
            if (args == null || args.Count != 3)
                throw new ArgumentException("DATEADD requires exactly 3 arguments.");

            var interval = args[0]?.ToString()?.ToLower();
            if (!double.TryParse(args[1]?.ToString(), out var number))
                throw new ArgumentException("Second argument must be a number.");

            if (!DateTime.TryParse(args[2]?.ToString(), out var baseDate))
                throw new ArgumentException("Third argument must be a valid date.");

            switch (interval)
            {
                case "yyyy": return baseDate.AddYears((int)number);
                case "q": return baseDate.AddMonths((int)(number * 3));
                case "m": return baseDate.AddMonths((int)number);
                case "y": return baseDate.AddDays(number); // same as "d"
                case "d": return baseDate.AddDays(number);
                case "w": return baseDate.AddDays(number); // weekday = day
                case "ww": return baseDate.AddDays(number * 7);
                case "h": return baseDate.AddHours(number);
                case "n": return baseDate.AddMinutes(number);
                case "s": return baseDate.AddSeconds(number);
                default:
                    throw new ArgumentException($"Invalid interval '{interval}' for DATEADD.");
            }
        }

        public string Name => "DATEADD";
    }
}