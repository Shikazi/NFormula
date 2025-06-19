using NFormula.Default.Functions;
using NFormula.Default.Operator.Arithmetic;
using NFormula.Default.Operator.Comparison;
using NFormula.Default.Operator.Logical;

namespace NFormula
{
    public static class FormulaBuilderExtension
    {
        public static FormularBuilder AddFunction<T>(this FormularBuilder builder) where T : IFunction, new()
        {
            builder.AddFunction(new T());
            return builder;
        }

        public static FormularBuilder AddBinaryOperator<T>(this FormularBuilder builder)
            where T : IBinaryOperator, new()
        {
            builder.AddBinaryOperator(new T());
            return builder;
        }

        public static FormularBuilder AddUnaryOperator<T>(this FormularBuilder builder) where T : IUnaryOperator, new()
        {
            builder.AddUnaryOperator(new T());
            return builder;
        }

        public static FormularBuilder AddDefault(this FormularBuilder builder)
        {
            // Unary Operators
            builder.AddUnaryOperator<LogicalNotOperator>();

            // Binary Operators
            builder.AddBinaryOperator<LogicalAndOperator>();
            builder.AddBinaryOperator<LogicalOrOperator>();
            builder.AddBinaryOperator<AddOperator>();
            builder.AddBinaryOperator<SubtractOperator>();
            builder.AddBinaryOperator<MultiplyOperator>();
            builder.AddBinaryOperator<DivideOperator>();
            builder.AddBinaryOperator<StringEqualOperator>();
            builder.AddBinaryOperator<NumberEqualOperator>();
            builder.AddBinaryOperator<BooleanEqualOperator>();
            builder.AddBinaryOperator<GreaterThanOperator>();
            builder.AddBinaryOperator<GreaterThanOrEqualOperator>();
            builder.AddBinaryOperator<LessThanOperator>();
            builder.AddBinaryOperator<LessThanOrEqualOperator>();

            // Functions
            builder.AddFunction<IfBooleanFunction>();
            builder.AddFunction<IfArrayTimeFunction>();
            builder.AddFunction<IfDateTimeFunction>();
            builder.AddFunction<IfNumberFunction>();
            builder.AddFunction<IfStringFunction>();
            builder.AddFunction<DateDiff>();
            builder.AddFunction<DateAddFunction>();

            return builder;
        }
    }
}