namespace Smart.WinUI.Markup;

using Microsoft.UI.Xaml.Markup;

[MarkupExtensionReturnType(ReturnType = typeof(double))]
public sealed partial class DoubleExtension : MarkupExtension
{
    public double Value { get; set; }

    protected override object ProvideValue() => Value;
}
