namespace Smart.WinUI.Data;

using Microsoft.UI.Xaml.Data;

public sealed partial class ReverseConverter : IValueConverter
{
    private static readonly object BoxedTrue = true;
    private static readonly object BoxedFalse = false;

    public object? Convert(object? value, Type targetType, object? parameter, string? language)
    {
        return value is bool boolValue ? (boolValue ? BoxedFalse : BoxedTrue) : value;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, string? language)
    {
        return value is bool boolValue ? (boolValue ? BoxedFalse : BoxedTrue) : value;
    }
}
