namespace Smart.WinUI.Data;

using System.Collections.ObjectModel;

using Microsoft.UI;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;

using Smart.Linq;

using Windows.UI;

public abstract class MapEntry<T>
{
    public object Key { get; set; } = default!;

    public T Value { get; set; } = default!;
}

public abstract class MapToObjectConverter<T> : IValueConverter
{
    public Collection<MapEntry<T>> Entries { get; } = [];

    public T DefaultValue { get; set; } = default!;

    public object? Convert(object? value, Type targetType, object? parameter, string? language)
    {
        if (value is not null)
        {
            if (value is IComparable comparable)
            {
                var entry = Entries.FirstOrDefault(comparable, static (x, s) => s.GetType().IsInstanceOfType(x.Key) && (s.CompareTo(x.Key) == 0));
                if (entry is not null)
                {
                    return entry.Value;
                }
            }
            else
            {
                var entry = Entries.FirstOrDefault(value, static (x, s) => Equals(s, x.Key));
                if (entry is not null)
                {
                    return entry.Value;
                }
            }
        }

        return DefaultValue;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, string? language)
    {
        throw new NotSupportedException();
    }
}

public sealed class MapToBrushEntry : MapEntry<Brush>;

public sealed partial class MapToBrushConverter : MapToObjectConverter<Brush>
{
    public MapToBrushConverter()
    {
        DefaultValue = new SolidColorBrush(Colors.Transparent);
    }
}

public sealed class MapToBoolEntry : MapEntry<bool>;

public sealed partial class MapToBoolConverter : MapToObjectConverter<bool>;

public sealed class MapToTextEntry : MapEntry<string?>;

public sealed partial class MapToTextConverter : MapToObjectConverter<string?>;

public sealed class MapToColorEntry : MapEntry<Color>;

public sealed partial class MapToColorConverter : MapToObjectConverter<Color>
{
    public MapToColorConverter()
    {
        DefaultValue = Colors.Transparent;
    }
}
