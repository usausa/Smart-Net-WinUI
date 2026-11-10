namespace Smart.WinUI.Markup;

using Microsoft.UI.Xaml.Markup;

using Smart.WinUI.Expressions;

[MarkupExtensionReturnType(ReturnType = typeof(ICompareExpression))]
public sealed partial class CompareExpressionExtension : MarkupExtension
{
    public CompareOperator Value { get; set; }

    protected override object ProvideValue() => Value switch
    {
        CompareOperator.NotEqual => CompareExpressions.NotEqual,
        CompareOperator.LessThan => CompareExpressions.LessThan,
        CompareOperator.LessThanOrEqual => CompareExpressions.LessThanOrEqual,
        CompareOperator.GreaterThan => CompareExpressions.GreaterThan,
        CompareOperator.GreaterThanOrEqual => CompareExpressions.GreaterThanOrEqual,
        _ => CompareExpressions.Equal
    };
}
