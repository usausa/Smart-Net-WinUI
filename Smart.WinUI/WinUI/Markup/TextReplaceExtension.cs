namespace Smart.WinUI.Markup;

using System.Text.RegularExpressions;

using Microsoft.UI.Xaml.Markup;

using Smart.WinUI.Data;

[MarkupExtensionReturnType(ReturnType = typeof(TextReplaceConverter))]
public sealed partial class TextReplaceExtension : MarkupExtension
{
    public string Pattern { get; set; } = string.Empty;

    public string Replacement { get; set; } = string.Empty;

    public RegexOptions Options { get; set; }

    public bool ReplaceAll { get; set; } = true;

    protected override object ProvideValue() =>
        new TextReplaceConverter { Pattern = Pattern, Replacement = Replacement, Options = Options, ReplaceAll = ReplaceAll };
}
