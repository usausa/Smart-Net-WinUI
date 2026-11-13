namespace Smart.WinUI.Data;

using System.Diagnostics.CodeAnalysis;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

using Smart.Converter;

[RequiresUnreferencedCode("Uses ObjectConverter, which uses reflection to discover types at runtime.")]
[RequiresDynamicCode("Uses ObjectConverter, which uses MakeGenericType/MakeGenericMethod at runtime.")]
public sealed partial class ObjectConvertConverter : IValueConverter
{
    public IObjectConverter Converter { get; set; } = ObjectConverter.Default;

    public object? Convert(object? value, Type targetType, object? parameter, string? language)
    {
        return Converter.Convert(value, targetType);
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, string? language)
    {
        if (Converter.TryConvert(value, targetType, out var result))
        {
            return result;
        }

        if (Converter.CanConvert(value, targetType))
        {
            return DependencyProperty.UnsetValue;
        }

        return Converter.Convert(value, targetType);
    }
}
