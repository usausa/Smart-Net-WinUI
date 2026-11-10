namespace Smart.WinUI.Data;

using Microsoft.UI.Xaml.Data;

public abstract class ObjectToBoolConverter<T> : IValueConverter
{
    public T TrueValue { get; set; } = default!;

    public T FalseValue { get; set; } = default!;

    public object Convert(object? value, Type targetType, object? parameter, string? language)
    {
        return (value is T typedValue) && Equals(typedValue, TrueValue);
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, string? language)
    {
        return value is true ? TrueValue : FalseValue;
    }
}

public sealed partial class TextToBoolConverter : ObjectToBoolConverter<string?>;

public sealed partial class IntToBoolConverter : ObjectToBoolConverter<int>;
