namespace Smart.WinUI.Markup;

using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Markup;
using Microsoft.UI.Xaml.Media;

using Smart.WinUI.Data;

using Windows.UI;

[MarkupExtensionReturnType(ReturnType = typeof(BoolToTextConverter))]
public sealed partial class BoolToTextExtension : MarkupExtension
{
    public string? True { get; set; }

    public string? False { get; set; }

    protected override object ProvideValue() =>
        new BoolToTextConverter { TrueValue = True, FalseValue = False };
}

[MarkupExtensionReturnType(ReturnType = typeof(BoolToVisibilityConverter))]
public sealed partial class BoolToVisibilityExtension : MarkupExtension
{
    public Visibility True { get; set; }

    public Visibility False { get; set; }

    protected override object ProvideValue() =>
        new BoolToVisibilityConverter { TrueValue = True, FalseValue = False };
}

[MarkupExtensionReturnType(ReturnType = typeof(BoolToBrushConverter))]
public sealed partial class BoolToBrushExtension : MarkupExtension
{
    public BoolToBrushExtension()
    {
        var transparent = new SolidColorBrush(Colors.Transparent);
        True = transparent;
        False = transparent;
    }

    public Brush True { get; set; }

    public Brush False { get; set; }

    protected override object ProvideValue() =>
        new BoolToBrushConverter { TrueValue = True, FalseValue = False };
}

[MarkupExtensionReturnType(ReturnType = typeof(BoolToColorConverter))]
public sealed partial class BoolToColorExtension : MarkupExtension
{
    public Color True { get; set; } = Colors.Transparent;

    public Color False { get; set; } = Colors.Transparent;

    protected override object ProvideValue() =>
        new BoolToColorConverter { TrueValue = True, FalseValue = False };
}
