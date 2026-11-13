namespace Smart.WinUI.Data;

using System.Collections;

using Microsoft.UI.Xaml.Data;

public sealed partial class ArrayIndexConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, string? language)
    {
        if ((value is int index) && (index >= 0))
        {
            if ((parameter is Array array) && (index < array.Length))
            {
                return array.GetValue(index);
            }

            if ((parameter is IList list) && (index < list.Count))
            {
                return list[index];
            }
        }

        return null;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, string? language)
    {
        return parameter is IList list ? list.IndexOf(value) : -1;
    }
}
