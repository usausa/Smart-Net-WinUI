namespace Smart.WinUI.Markup;

using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Markup;
using Microsoft.UI.Xaml.Media;

using Smart.WinUI.Data;
using Smart.WinUI.Expressions;

using Windows.UI;

[MarkupExtensionReturnType(ReturnType = typeof(CompareToBoolConverter))]
public sealed partial class CompareToBoolExtension : MarkupExtension
{
    public ICompareExpression? Expression { get; set; }

    protected override object ProvideValue() =>
        new CompareToBoolConverter { Expression = Expression ?? CompareExpressions.Equal, TrueValue = true, FalseValue = false };
}

[MarkupExtensionReturnType(ReturnType = typeof(CompareToTextConverter))]
public sealed partial class CompareToTextExtension : MarkupExtension
{
    public ICompareExpression? Expression { get; set; }

    public string? True { get; set; }

    public string? False { get; set; }

    protected override object ProvideValue() =>
        new CompareToTextConverter { Expression = Expression ?? CompareExpressions.Equal, TrueValue = True, FalseValue = False };
}

[MarkupExtensionReturnType(ReturnType = typeof(CompareToVisibilityConverter))]
public sealed partial class CompareToVisibilityExtension : MarkupExtension
{
    public ICompareExpression? Expression { get; set; }

    public Visibility True { get; set; }

    public Visibility False { get; set; }

    protected override object ProvideValue() =>
        new CompareToVisibilityConverter { Expression = Expression ?? CompareExpressions.Equal, TrueValue = True, FalseValue = False };
}

[MarkupExtensionReturnType(ReturnType = typeof(CompareToBrushConverter))]
public sealed partial class CompareToBrushExtension : MarkupExtension
{
    public CompareToBrushExtension()
    {
        var transparent = new SolidColorBrush(Colors.Transparent);
        True = transparent;
        False = transparent;
    }

    public ICompareExpression? Expression { get; set; }

    public Brush True { get; set; }

    public Brush False { get; set; }

    protected override object ProvideValue() =>
        new CompareToBrushConverter { Expression = Expression ?? CompareExpressions.Equal, TrueValue = True, FalseValue = False };
}

[MarkupExtensionReturnType(ReturnType = typeof(CompareToColorConverter))]
public sealed partial class CompareToColorExtension : MarkupExtension
{
    public ICompareExpression? Expression { get; set; }

    public Color True { get; set; } = Colors.Transparent;

    public Color False { get; set; } = Colors.Transparent;

    protected override object ProvideValue() =>
        new CompareToColorConverter { Expression = Expression ?? CompareExpressions.Equal, TrueValue = True, FalseValue = False };
}
