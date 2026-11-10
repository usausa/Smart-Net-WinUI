namespace Smart.WinUI.Data;

using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;

using Smart.WinUI.Expressions;

using Windows.UI;

public abstract class CompareConverter<T> : IValueConverter
{
    public ICompareExpression Expression { get; set; } = CompareExpressions.Equal;

    public T TrueValue { get; set; } = default!;

    public T FalseValue { get; set; } = default!;

    public object? Convert(object? value, Type targetType, object? parameter, string? language)
    {
        return Expression.Eval(value, parameter) ? TrueValue : FalseValue;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, string? language)
    {
        throw new NotSupportedException();
    }
}

public sealed partial class CompareToBoolConverter : CompareConverter<bool>
{
    public CompareToBoolConverter()
    {
        TrueValue = true;
        FalseValue = false;
    }
}

public sealed partial class CompareToTextConverter : CompareConverter<string?>;

public sealed partial class CompareToVisibilityConverter : CompareConverter<Visibility>;

public sealed partial class CompareToBrushConverter : CompareConverter<Brush>
{
    public CompareToBrushConverter()
    {
        var transparent = new SolidColorBrush(Colors.Transparent);
        TrueValue = transparent;
        FalseValue = transparent;
    }
}

public sealed partial class CompareToColorConverter : CompareConverter<Color>
{
    public CompareToColorConverter()
    {
        TrueValue = Colors.Transparent;
        FalseValue = Colors.Transparent;
    }
}
