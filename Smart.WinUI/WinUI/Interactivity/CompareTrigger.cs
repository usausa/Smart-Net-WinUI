namespace Smart.WinUI.Interactivity;

using Microsoft.UI.Xaml;
using Microsoft.Xaml.Interactivity;

using Smart.WinUI.Expressions;

[TypeConstraint(typeof(DependencyObject))]
public sealed class CompareTrigger : Trigger
{
    public static readonly DependencyProperty BindingProperty = DependencyProperty.Register(
        nameof(Binding),
        typeof(object),
        typeof(CompareTrigger),
        new PropertyMetadata(null, HandlePropertyChanged));

    public static readonly DependencyProperty ParameterProperty = DependencyProperty.Register(
        nameof(Parameter),
        typeof(object),
        typeof(CompareTrigger),
        new PropertyMetadata(null, HandlePropertyChanged));

    public static readonly DependencyProperty ExpressionProperty = DependencyProperty.Register(
        nameof(Expression),
        typeof(ICompareExpression),
        typeof(CompareTrigger),
        new PropertyMetadata(CompareExpressions.Equal, HandlePropertyChanged));

    public object? Binding
    {
        get => GetValue(BindingProperty);
        set => SetValue(BindingProperty, value);
    }

    public object? Parameter
    {
        get => GetValue(ParameterProperty);
        set => SetValue(ParameterProperty, value);
    }

    public ICompareExpression? Expression
    {
        get => (ICompareExpression?)GetValue(ExpressionProperty);
        set => SetValue(ExpressionProperty, value);
    }

    private static void HandlePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (e.OldValue == e.NewValue)
        {
            return;
        }

        ((CompareTrigger)d).HandlePropertyChanged();
    }

    private void HandlePropertyChanged()
    {
        if (AssociatedObject is null)
        {
            return;
        }

        var expression = Expression ?? CompareExpressions.Equal;
        if (expression.Eval(Binding, Parameter))
        {
            Interaction.ExecuteActions(AssociatedObject, Actions, null);
        }
    }
}
