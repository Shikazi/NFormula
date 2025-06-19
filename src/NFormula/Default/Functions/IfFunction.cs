using System;
using System.Collections.Generic;

namespace NFormula.Default.Functions
{
    public class IfStringFunction : IFunction
    {
        public DataType[] ParameterTypes { get; } =
            { DataType.Boolean, DataType.String, DataType.String };

        public DataType ReturnType => DataType.String;

        public object Evaluate(IList<object> args)
        {
            if (args == null || args.Count != 3)
                throw new ArgumentException("IF requires exactly 3 arguments.");

            var condition = args[0];
            var trueResult = args[1]?.ToString();
            var falseResult = args[2]?.ToString();

            if (condition is bool b)
                return b ? trueResult : falseResult;

            throw new ArgumentException("First argument of IF must be a boolean.");
        }

        public string Name => "IF";
    }

    public class IfNumberFunction : IFunction
    {
        public DataType[] ParameterTypes { get; } =
            { DataType.Boolean, DataType.Number, DataType.Number };

        public DataType ReturnType => DataType.Number;

        public object Evaluate(IList<object> args)
        {
            if (args == null || args.Count != 3)
                throw new ArgumentException("IF requires exactly 3 arguments.");

            var condition = args[0];

            if (condition is bool b)
                return b ? args[1] : args[2];

            throw new ArgumentException("First argument of IF must be a boolean.");
        }

        public string Name => "IF";
    }

    public class IfBooleanFunction : IFunction
    {
        public DataType[] ParameterTypes { get; } =
            { DataType.Boolean, DataType.Boolean, DataType.Boolean };

        public DataType ReturnType => DataType.Boolean;

        public object Evaluate(IList<object> args)
        {
            if (args == null || args.Count != 3)
                throw new ArgumentException("IF requires exactly 3 arguments.");

            var condition = args[0];

            if (condition is bool b)
                return b ? args[1] : args[2];

            throw new ArgumentException("First argument of IF must be a boolean.");
        }

        public string Name => "IF";
    }

    public class IfDateTimeFunction : IFunction
    {
        public DataType[] ParameterTypes { get; } =
            { DataType.Boolean, DataType.DateTime, DataType.DateTime };

        public DataType ReturnType => DataType.DateTime;

        public object Evaluate(IList<object> args)
        {
            if (args == null || args.Count != 3)
                throw new ArgumentException("IF requires exactly 3 arguments.");

            var condition = args[0];

            if (condition is bool b)
                return b ? args[1] : args[2];

            throw new ArgumentException("First argument of IF must be a boolean.");
        }

        public string Name => "IF";
    }

    public class IfArrayTimeFunction : IFunction
    {
        public DataType[] ParameterTypes { get; } =
            { DataType.Boolean, DataType.Array, DataType.Array };

        public DataType ReturnType => DataType.Array;

        public object Evaluate(IList<object> args)
        {
            if (args == null || args.Count != 3)
                throw new ArgumentException("IF requires exactly 3 arguments.");

            var condition = args[0];

            if (condition is bool b)
                return b ? args[1] : args[2];

            throw new ArgumentException("First argument of IF must be a boolean.");
        }

        public string Name => "IF";
    }
}