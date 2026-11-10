namespace Smart.WinUI.Markup;

using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Markup;
using Microsoft.UI.Xaml.Media;

using Smart.WinUI.Data;

using Windows.UI;

[MarkupExtensionReturnType(ReturnType = typeof(ContainsToBoolConverter))]
public sealed partial class ContainsToBoolExtension : MarkupExtension
{
    public bool Invert { get; set; }

    protected override object ProvideValue() =>
        new ContainsToBoolConverter { TrueValue = !Invert, FalseValue = Invert };
}

[MarkupExtensionReturnType(ReturnType = typeof(ContainsToTextConverter))]
public sealed partial class ContainsToTextExtension : MarkupExtension
{
    public string? True { get; set; }

    public string? False { get; set; }

    protected override object ProvideValue() =>
        new ContainsToTextConverter { TrueValue = True, FalseValue = False };
}

[MarkupExtensionReturnType(ReturnType = typeof(ContainsToVisibilityConverter))]
public sealed partial class ContainsToVisibilityExtension : MarkupExtension
{
    public Visibility True { get; set; }

    public Visibility False { get; set; }

    protected override object ProvideValue() =>
        new ContainsToVisibilityConverter { TrueValue = True, FalseValue = False };
}

[MarkupExtensionReturnType(ReturnType = typeof(ContainsToBrushConverter))]
public sealed partial class ContainsToBrushExtension : MarkupExtension
{
    public ContainsToBrushExtension()
    {
        var transparent = new SolidColorBrush(Colors.Transparent);
        True = transparent;
        False = transparent;
    }

    public Brush True { get; set; }

    public Brush False { get; set; }

    protected override object ProvideValue() =>
        new ContainsToBrushConverter { TrueValue = True, FalseValue = False };
}

[MarkupExtensionReturnType(ReturnType = typeof(ContainsToColorConverter))]
public sealed partial class ContainsToColorExtension : MarkupExtension
{
    public Color True { get; set; } = Colors.Transparent;

    public Color False { get; set; } = Colors.Transparent;

    protected override object ProvideValue() =>
        new ContainsToColorConverter { TrueValue = True, FalseValue = False };
}
