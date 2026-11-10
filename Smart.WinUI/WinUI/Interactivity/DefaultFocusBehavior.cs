namespace Smart.WinUI.Interactivity;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.Xaml.Interactivity;

[TypeConstraint(typeof(UIElement))]
public sealed class DefaultFocusBehavior : Behavior<UIElement>
{
    protected override void OnAttached()
    {
        base.OnAttached();

        if (AssociatedObject is FrameworkElement { IsLoaded: false } element)
        {
            element.Loaded += OnLoaded;
        }
        else
        {
            FocusTarget();
        }
    }

    protected override void OnDetaching()
    {
        if (AssociatedObject is FrameworkElement element)
        {
            element.Loaded -= OnLoaded;
        }

        base.OnDetaching();
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        ((FrameworkElement)sender).Loaded -= OnLoaded;
        FocusTarget();
    }

    private void FocusTarget()
    {
        var target = FindTarget(AssociatedObject) ?? AssociatedObject;
        target.Focus(FocusState.Programmatic);
    }

    private static UIElement? FindTarget(DependencyObject parent)
    {
        var count = VisualTreeHelper.GetChildrenCount(parent);
        for (var i = 0; i < count; i++)
        {
            if ((VisualTreeHelper.GetChild(parent, i) is not UIElement { Visibility: Visibility.Visible } child) ||
                (child is Control { IsEnabled: false }))
            {
                continue;
            }

            if (DefaultFocus.GetIsTarget(child))
            {
                return child;
            }

            var target = FindTarget(child);
            if (target is not null)
            {
                return target;
            }
        }

        return null;
    }
}
