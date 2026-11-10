namespace Smart.WinUI.Markup;

using Microsoft.UI.Xaml.Markup;

[MarkupExtensionReturnType(ReturnType = typeof(bool))]
public sealed partial class BoolExtension : MarkupExtension
{
    public bool Value { get; set; }

    protected override object ProvideValue() => Value;
}
