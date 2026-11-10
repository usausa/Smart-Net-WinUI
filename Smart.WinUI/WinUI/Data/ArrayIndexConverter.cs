namespace Smart.WinUI.Data;

using Microsoft.UI.Xaml.Data;

public sealed partial class ArrayIndexConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, string? language)
    {
        if ((value is int index) && (parameter is Array array) && (index >= 0) && (index < array.Length))
        {
            return array.GetValue(index);
        }

        return null;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, string? language)
    {
        if (parameter is Array array)
        {
            for (var i = 0; i < array.Length; i++)
            {
                var element = array.GetValue(i);
                if (((element is not null) && element.Equals(value)) ||
                    ((element is null) && (value is null)))
                {
                    return i;
                }
            }
        }

        return -1;
    }
}
