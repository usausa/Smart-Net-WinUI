namespace Smart.WinUI.Data;

using Microsoft.UI.Xaml.Data;

public sealed partial class NullToParameterConverter : IValueConverter
{
    public bool Invert { get; set; }

    public bool HandleEmptyString { get; set; }

    public object? Convert(object? value, Type targetType, object? parameter, string? language)
    {
        if ((value is null) ||
            (HandleEmptyString && (value is string { Length: 0 })))
        {
            return Invert ? value : parameter;
        }

        return Invert ? parameter : value;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, string? language)
    {
        throw new NotSupportedException();
    }
}
