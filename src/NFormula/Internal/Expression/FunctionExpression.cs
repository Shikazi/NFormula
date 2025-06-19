namespace NFormula.Internal.Expression
{
    /// <summary>
    /// Represents a function call in the formula.
    /// </summary>
    internal sealed class FunctionExpression : IFormulaExpression
    {
        private readonly IFunction _function;
        private readonly IFormulaExpression[] _arguments;

        public FunctionExpression(IFunction function, IFormulaExpression[] arguments)
        {
            _function = function;
            _arguments = arguments;
        }

        public DataType ReturnType => _function.ReturnType;

        public object Evaluate(IEvaluationContext context)
        {
            var args = new object[_arguments.Length];
            for (var i = 0; i < _arguments.Length; i++)
            {
                args[i] = _arguments[i].Evaluate(context);
            }

            return _function.Evaluate(args);
        }
    }
}