namespace Smart.WinUI.Data;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

public sealed partial class ParameterEqualsConverter : IValueConverter
{
    private static readonly object BoxedTrue = true;
    private static readonly object BoxedFalse = false;

    public object Convert(object? value, Type targetType, object? parameter, string? language)
    {
        return Equals(value, parameter) ? BoxedTrue : BoxedFalse;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, string? language)
    {
        return Equals(value, true) ? parameter : DependencyProperty.UnsetValue;
    }
}
