namespace Smart.WinUI.Markup;

using Microsoft.UI.Xaml.Markup;

using Smart.WinUI.Data;

using Windows.UI;

[MarkupExtensionReturnType(ReturnType = typeof(ColorBlendConverter))]
public sealed partial class ColorBlendExtension : MarkupExtension
{
    public Color Color { get; set; }

    public double Raito { get; set; }

    protected override object ProvideValue() =>
        new ColorBlendConverter { Color = Color, Raito = Raito };
}
