namespace Smart.WinUI.Expressions;

public interface ICompareExpression
{
    bool Eval(object? left, object? right);
}
