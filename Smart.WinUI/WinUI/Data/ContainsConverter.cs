namespace Smart.WinUI.Data;

using System.Collections;

using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;

using Windows.UI;

public abstract class ContainsConverter<T> : IValueConverter
{
    public T TrueValue { get; set; } = default!;

    public T FalseValue { get; set; } = default!;

    public object? Convert(object? value, Type targetType, object? parameter, string? language)
    {
        return (parameter is IList list) && list.Contains(value) ? TrueValue : FalseValue;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, string? language)
    {
        throw new NotSupportedException();
    }
}

public sealed partial class ContainsToBoolConverter : ContainsConverter<bool>
{
    public ContainsToBoolConverter()
    {
        TrueValue = true;
        FalseValue = false;
    }
}

public sealed partial class ContainsToTextConverter : ContainsConverter<string?>;

public sealed partial class ContainsToVisibilityConverter : ContainsConverter<Visibility>;

public sealed partial class ContainsToBrushConverter : ContainsConverter<Brush>
{
    public ContainsToBrushConverter()
    {
        var transparent = new SolidColorBrush(Colors.Transparent);
        TrueValue = transparent;
        FalseValue = transparent;
    }
}

public sealed partial class ContainsToColorConverter : ContainsConverter<Color>
{
    public ContainsToColorConverter()
    {
        TrueValue = Colors.Transparent;
        FalseValue = Colors.Transparent;
    }
}
