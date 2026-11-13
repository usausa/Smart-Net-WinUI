namespace Smart.WinUI.Data;

using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;

using Windows.UI;

public abstract class BoolToObjectConverter<T> : IValueConverter
{
    public T TrueValue { get; set; } = default!;

    public T FalseValue { get; set; } = default!;

    public object? Convert(object? value, Type targetType, object? parameter, string? language)
    {
        if (value is bool boolValue)
        {
            return boolValue ? TrueValue : FalseValue;
        }

        return FalseValue;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, string? language)
    {
        if (value is T typed)
        {
            if (typed is IEquatable<T> equatable)
            {
                if (equatable.Equals(TrueValue))
                {
                    return true;
                }
                if (equatable.Equals(FalseValue))
                {
                    return false;
                }
            }
            else
            {
                if (Equals(typed, TrueValue))
                {
                    return true;
                }
                if (Equals(typed, FalseValue))
                {
                    return false;
                }
            }
        }

        return DependencyProperty.UnsetValue;
    }
}

public sealed partial class BoolToTextConverter : BoolToObjectConverter<string?>;

public sealed partial class BoolToVisibilityConverter : BoolToObjectConverter<Visibility>;

public sealed partial class BoolToBrushConverter : BoolToObjectConverter<Brush>
{
    public BoolToBrushConverter()
    {
        var transparent = new SolidColorBrush(Colors.Transparent);
        TrueValue = transparent;
        FalseValue = transparent;
    }
}

public sealed partial class BoolToColorConverter : BoolToObjectConverter<Color>
{
    public BoolToColorConverter()
    {
        TrueValue = Colors.Transparent;
        FalseValue = Colors.Transparent;
    }
}
