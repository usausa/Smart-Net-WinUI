namespace Smart.WinUI.Resolver;

using System.Diagnostics.CodeAnalysis;

using Microsoft.UI.Xaml.Markup;

public sealed partial class ResolveExtension : MarkupExtension
{
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
    public Type Type { get; set; } = default!;

    protected override object ProvideValue() => ResolveHelper.Resolve(Type);
}
