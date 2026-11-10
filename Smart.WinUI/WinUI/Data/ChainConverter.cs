namespace Smart.WinUI.Data;

using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Markup;

[ContentProperty(Name = "Converters")]
public sealed partial class ChainConverter : IValueConverter
{
    // ReSharper disable once CollectionNeverUpdated.Global
    public Collection<IValueConverter> Converters { get; } = [];

    public object? Convert(object? value, Type targetType, object? parameter, string? language)
    {
        var result = value;
        // ReSharper disable once ForCanBeConvertedToForeach
        for (var i = 0; i < Converters.Count; i++)
        {
            result = Converters[i].Convert(result, targetType, parameter, language);
            if (IsSentinel(result))
            {
                break;
            }
        }

        return result;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, string? language)
    {
        var result = value;
        for (var i = Converters.Count - 1; i >= 0; i--)
        {
            result = Converters[i].ConvertBack(result, targetType, parameter, language);
            if (IsSentinel(result))
            {
                break;
            }
        }

        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsSentinel(object? value) =>
        value == DependencyProperty.UnsetValue;
}
