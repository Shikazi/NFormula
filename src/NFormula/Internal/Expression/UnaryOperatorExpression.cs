using System;
using System.Collections.Generic;
using System.Linq;

namespace NFormula.Internal.Expression
{
    /// <summary>
    /// Represents a unary operator expression (e.g. -x, !flag).
    /// </summary>
    internal sealed class UnaryOperatorExpression : IFormulaExpression
    {
        private readonly IFormulaExpression _operand;
        private readonly IEnumerable<IUnaryOperator> _availableOperator;

        public UnaryOperatorExpression(IFormulaExpression operand, IEnumerable<IUnaryOperator> availableOperator)
        {
            _operand = operand;
            _availableOperator = availableOperator;
        }


        public object Evaluate(IEvaluationContext context)
        {
            var op = GetMatchOperator(context);
            var value = _operand.Evaluate(context);
            return op.Evaluate(value);
        }

        public DataType GetReturnType(IDataTypeContext context)
        {
            var matchFunction = GetMatchOperator(context);
            if (matchFunction == null)
                throw new Exception("Can't find matching Operator");
            return matchFunction.ReturnType;
        }

        private IUnaryOperator GetMatchOperator(IDataTypeContext context)
        {
            var operandDataType = _operand.GetReturnType(context);
            return _availableOperator.FirstOrDefault(x => x.OperandType == operandDataType);
        }
    }
}