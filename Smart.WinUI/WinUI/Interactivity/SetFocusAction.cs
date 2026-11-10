namespace Smart.WinUI.Interactivity;

using Microsoft.UI.Xaml;
using Microsoft.Xaml.Interactivity;

public sealed class SetFocusAction : DependencyObject, IAction
{
    public static readonly DependencyProperty TargetObjectProperty = DependencyProperty.Register(
        nameof(TargetObject),
        typeof(FrameworkElement),
        typeof(SetFocusAction),
        new PropertyMetadata(null));

    public FrameworkElement? TargetObject
    {
        get => (FrameworkElement?)GetValue(TargetObjectProperty);
        set => SetValue(TargetObjectProperty, value);
    }

    public object Execute(object? sender, object? parameter)
    {
        var element = TargetObject ?? (sender as FrameworkElement);
        return (element is not null) && element.Focus(FocusState.Programmatic);
    }
}
