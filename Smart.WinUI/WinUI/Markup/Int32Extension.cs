namespace Smart.WinUI.Markup;

using Microsoft.UI.Xaml.Markup;

[MarkupExtensionReturnType(ReturnType = typeof(int))]
public sealed partial class Int32Extension : MarkupExtension
{
    public int Value { get; set; }

    protected override object ProvideValue() => Value;
}
