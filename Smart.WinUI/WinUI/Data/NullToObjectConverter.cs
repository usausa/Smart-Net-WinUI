namespace Smart.WinUI.Data;

using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;

using Windows.UI;

public abstract class NullToObjectConverter<T> : IValueConverter
{
    public T NullValue { get; set; } = default!;

    public T NonNullValue { get; set; } = default!;

    public bool HandleEmptyString { get; set; }

    public object? Convert(object? value, Type targetType, object? parameter, string? language)
    {
        if ((value is null) ||
            (HandleEmptyString && (value is string { Length: 0 })))
        {
            return NullValue;
        }

        return NonNullValue;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, string? language)
    {
        throw new NotSupportedException();
    }
}

public sealed partial class NullToBoolConverter : NullToObjectConverter<bool>
{
    public NullToBoolConverter()
    {
        NullValue = false;
        NonNullValue = true;
    }
}

public sealed partial class NullToTextConverter : NullToObjectConverter<string?>;

public sealed partial class NullToVisibilityConverter : NullToObjectConverter<Visibility>;

public sealed partial class NullToBrushConverter : NullToObjectConverter<Brush>
{
    public NullToBrushConverter()
    {
        var transparent = new SolidColorBrush(Colors.Transparent);
        NullValue = transparent;
        NonNullValue = transparent;
    }
}

public sealed partial class NullToColorConverter : NullToObjectConverter<Color>
{
    public NullToColorConverter()
    {
        NullValue = Colors.Transparent;
        NonNullValue = Colors.Transparent;
    }
}
