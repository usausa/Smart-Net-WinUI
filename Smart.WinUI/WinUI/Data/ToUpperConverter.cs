namespace Smart.WinUI.Data;

using Microsoft.UI.Xaml.Data;

using Smart.WinUI.Internal;

public sealed partial class ToUpperConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, string? language)
    {
        return value is string text ? text.ToUpper(LanguageCulture.Resolve(language)) : null;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, string? language)
    {
        throw new NotSupportedException();
    }
}
