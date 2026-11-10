namespace Smart.WinUI.Data;

using System.Collections.Concurrent;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

using Microsoft.UI.Xaml.Data;

public sealed partial class EnumDescriptionConverter : IValueConverter
{
    private static readonly ConcurrentDictionary<(Type, object), string> Cache = new();

    public object? Convert(object? value, Type targetType, object? parameter, string? language)
    {
        if (value is null)
        {
            return null;
        }

        if (value is Enum)
        {
            return Cache.GetOrAdd((value.GetType(), value), static key => GetDescription(key.Item2));
        }

        return value.ToString();
    }

    [UnconditionalSuppressMessage("Trimming", "IL2075", Justification = "Enum type members are expected to be preserved by the application")]
    private static string GetDescription(object value)
    {
        var type = value.GetType();
        var mis = type.GetMember(value.ToString()!);
        if (mis.Length > 0)
        {
            var attr = mis[0].GetCustomAttribute<DescriptionAttribute>();
            if (attr is not null)
            {
                return attr.Description;
            }
        }

        return value.ToString()!;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, string? language)
    {
        throw new NotSupportedException();
    }
}
