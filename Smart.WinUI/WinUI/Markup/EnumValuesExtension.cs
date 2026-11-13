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

    protected override object ProvideValue()
    {
        var values = Enum.GetValuesAsUnderlyingType(Type);
        var result = new object[values.Length];
        for (var i = 0; i < values.Length; i++)
        {
            result[i] = Enum.ToObject(Type, values.GetValue(i)!);
        }

        return result;
    }
}
