namespace Smart.WinUI.Markup;

using Microsoft.UI.Xaml.Markup;

using Smart.WinUI.Data;

[MarkupExtensionReturnType(ReturnType = typeof(TextToBoolConverter))]
public sealed partial class TextToBoolExtension : MarkupExtension
{
    public string? True { get; set; }

    public string? False { get; set; }

    protected override object ProvideValue() =>
        new TextToBoolConverter { TrueValue = True, FalseValue = False };
}

[MarkupExtensionReturnType(ReturnType = typeof(IntToBoolConverter))]
public sealed partial class IntToBoolExtension : MarkupExtension
{
    public int True { get; set; }

    public int False { get; set; }

    protected override object ProvideValue() =>
        new IntToBoolConverter { TrueValue = True, FalseValue = False };
}
