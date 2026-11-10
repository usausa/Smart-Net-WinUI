namespace Smart.WinUI.Markup;

using Microsoft.UI.Xaml.Markup;

using Smart.WinUI.Expressions;

[MarkupExtensionReturnType(ReturnType = typeof(IBinaryExpression))]
public sealed partial class BinaryExpressionExtension : MarkupExtension
{
    public BinaryOperator Value { get; set; }

    protected override object ProvideValue() => Value switch
    {
        BinaryOperator.Min => BinaryExpressions.Min,
        BinaryOperator.Add => BinaryExpressions.Add,
        BinaryOperator.Sub => BinaryExpressions.Sub,
        _ => BinaryExpressions.Max
    };
}
