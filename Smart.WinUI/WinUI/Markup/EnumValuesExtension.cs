namespace Smart.WinUI.Markup;

using System.Collections;
using System.Diagnostics.CodeAnalysis;

using Microsoft.UI.Xaml.Markup;

[MarkupExtensionReturnType(ReturnType = typeof(IEnumerable))]
public sealed partial class EnumValuesExtension : MarkupExtension
{
    [RequiresDynamicCode("Enum values as items need dynamic code in WinUI")]
    public EnumValuesExtension()
    {
    }

    public Type Type { get; set; } = default!;

    [UnconditionalSuppressMessage("Trimming", "IL2111", Justification = "Enum type is specified by user; they must ensure it is preserved")]
    [UnconditionalSuppressMessage("AOT", "IL3050", Justification = "Enum.GetValues(Type) uses dynamic code; use Enum.GetValues<TEnum>() for AOT-safe usage")]
    protected override object ProvideValue() => Enum.GetValues(Type);
}
