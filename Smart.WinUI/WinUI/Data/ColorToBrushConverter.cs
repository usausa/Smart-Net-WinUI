namespace Smart.WinUI.Data;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;

using Windows.UI;

public sealed partial class ColorToBrushConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, string? language)
    {
        return value is Color color ? new SolidColorBrush(color) : null;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, string? language)
    {
        return value is SolidColorBrush brush ? brush.Color : DependencyProperty.UnsetValue;
    }
}
