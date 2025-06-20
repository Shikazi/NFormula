using System;
using System.Collections.Generic;
using System.Linq;

namespace NFormula.Internal.Expression
{
    /// <summary>
    /// Represents a function call in the formula.
    /// </summary>
    internal sealed class FunctionExpression : IFormulaExpression
    {
        private readonly IEnumerable<IFunction> _availableFunctions;
        private readonly IFormulaExpression[] _arguments;

        public FunctionExpression(IEnumerable<IFunction> availableFunctions, IFormulaExpression[] arguments)
        {
            _arguments = arguments;
            _availableFunctions = availableFunctions;
        }

        private IFunction GetMatchFunction(IDataTypeContext context)
        {
            var argumentTypes = _arguments.Select(x => x.GetReturnType(context));
            return _availableFunctions.FirstOrDefault(x => x.ParameterTypes.SequenceEqual(argumentTypes));
        }

        public object Evaluate(IEvaluationContext context)
        {
            var matchFunction = GetMatchFunction(context);
            var args = new object[_arguments.Length];
            for (var i = 0; i < _arguments.Length; i++)
            {
                args[i] = _arguments[i].Evaluate(context);
            }

            return matchFunction.Evaluate(args);
        }

        public DataType GetReturnType(IDataTypeContext context)
        {
            var matchFunction = GetMatchFunction(context);
            if (matchFunction == null)
                throw new Exception("Can't find matching function");
            return matchFunction.ReturnType;
        }
    }
}