namespace Smart.WinUI.Data;

using Microsoft.UI.Xaml.Data;

public sealed partial class EnumDescriptionConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, string? language)
    {
        if (value is null)
        {
            return null;
        }

        if (value is Enum e)
        {
            return Enums.GetDescription(e);
        }

        return value.ToString();
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, string? language)
    {
        throw new NotSupportedException();
    }
}
