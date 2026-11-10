namespace Smart.WinUI.Markup;

using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Markup;
using Microsoft.UI.Xaml.Media;

using Smart.WinUI.Data;

using Windows.UI;

[MarkupExtensionReturnType(ReturnType = typeof(NullToBoolConverter))]
public sealed partial class NullToBoolExtension : MarkupExtension
{
    public bool Invert { get; set; }

    public bool HandleEmptyString { get; set; }

    protected override object ProvideValue() =>
        new NullToBoolConverter { NullValue = !Invert, NonNullValue = Invert, HandleEmptyString = HandleEmptyString };
}

[MarkupExtensionReturnType(ReturnType = typeof(NullToTextConverter))]
public sealed partial class NullToTextExtension : MarkupExtension
{
    public string? Null { get; set; }

    public string? NonNull { get; set; }

    public bool HandleEmptyString { get; set; }

    protected override object ProvideValue() =>
        new NullToTextConverter { NullValue = Null, NonNullValue = NonNull, HandleEmptyString = HandleEmptyString };
}

[MarkupExtensionReturnType(ReturnType = typeof(NullToVisibilityConverter))]
public sealed partial class NullToVisibilityExtension : MarkupExtension
{
    public Visibility Null { get; set; }

    public Visibility NonNull { get; set; }

    public bool HandleEmptyString { get; set; }

    protected override object ProvideValue() =>
        new NullToVisibilityConverter { NullValue = Null, NonNullValue = NonNull, HandleEmptyString = HandleEmptyString };
}

[MarkupExtensionReturnType(ReturnType = typeof(NullToBrushConverter))]
public sealed partial class NullToBrushExtension : MarkupExtension
{
    public NullToBrushExtension()
    {
        var transparent = new SolidColorBrush(Colors.Transparent);
        Null = transparent;
        NonNull = transparent;
    }

    public Brush Null { get; set; }

    public Brush NonNull { get; set; }

    public bool HandleEmptyString { get; set; }

    protected override object ProvideValue() =>
        new NullToBrushConverter { NullValue = Null, NonNullValue = NonNull, HandleEmptyString = HandleEmptyString };
}

[MarkupExtensionReturnType(ReturnType = typeof(NullToColorConverter))]
public sealed partial class NullToColorExtension : MarkupExtension
{
    public Color Null { get; set; } = Colors.Transparent;

    public Color NonNull { get; set; } = Colors.Transparent;

    public bool HandleEmptyString { get; set; }

    protected override object ProvideValue() =>
        new NullToColorConverter { NullValue = Null, NonNullValue = NonNull, HandleEmptyString = HandleEmptyString };
}
