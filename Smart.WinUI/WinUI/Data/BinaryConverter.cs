namespace Smart.WinUI.Data;

using Microsoft.UI.Xaml.Data;

using Smart.WinUI.Expressions;

public sealed partial class BinaryConverter : IValueConverter
{
    public IBinaryExpression Expression { get; set; } = default!;

    public object? Convert(object? value, Type targetType, object? parameter, string? language)
    {
        return Expression.Eval(value, parameter);
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, string? language)
    {
        throw new NotSupportedException();
    }
}
