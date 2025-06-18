using System.Collections.Generic;

namespace NFormula
{
    public class FormularBuilder
    {
        private readonly List<IFunction> _functions = new List<IFunction>();
        private readonly List<IUnaryOperator> _unaryOperators = new List<IUnaryOperator>();
        private readonly List<IBinaryOperator> _binaryOperators = new List<IBinaryOperator>();

        public FormularBuilder()
        {
        }

        public NFormularParser Build()
        {
            return new NFormularParser(_functions, _unaryOperators, _binaryOperators);
        }
    }
}