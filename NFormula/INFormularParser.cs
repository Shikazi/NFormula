namespace NFormula
{
    public interface INFormularParser
    {
        IFormulaExpression Parse(string input,IVariableTypeProfiler profiler);
    }
}