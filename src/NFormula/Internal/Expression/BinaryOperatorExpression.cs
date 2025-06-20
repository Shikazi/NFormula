using System;
using System.Collections.Generic;
using System.Linq;

namespace NFormula.Internal.Expression
{
    /// <summary>
    /// Represents a binary operator expression (e.g. a + b).
    /// </summary>
    internal sealed class BinaryOperatorExpression : IFormulaExpression
    {
        private readonly IFormulaExpression _left;
        private readonly IFormulaExpression _right;
        private readonly IEnumerable<IBinaryOperator> _availableOperator;

        public BinaryOperatorExpression(IFormulaExpression left, IFormulaExpression right,
            IEnumerable<IBinaryOperator> availableOperator)
        {
            _left = left;
            _right = right;
            _availableOperator = availableOperator;
        }

        private IBinaryOperator GetMatchOperator(IDataTypeContext context)
        {
            var leftType = _left.GetReturnType(context);
            var rightType = _right.GetReturnType(context);
            return _availableOperator.FirstOrDefault(x => x.LeftType == leftType && x.RightType == rightType);
        }

        public object Evaluate(IEvaluationContext context)
        {
            var leftValue = _left.Evaluate(context);
            var rightValue = _right.Evaluate(context);
            var op = GetMatchOperator(context);
            return op.Evaluate(leftValue, rightValue);
        }

        public DataType GetReturnType(IDataTypeContext context)
        {
            var matchFunction = GetMatchOperator(context);
            if (matchFunction == null)
                throw new Exception("Can't find matching Operator");
            return matchFunction.ReturnType;
        }
    }
}