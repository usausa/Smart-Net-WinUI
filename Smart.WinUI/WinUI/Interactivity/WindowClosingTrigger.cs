namespace Smart.WinUI.Interactivity;

using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.Xaml.Interactivity;

using Smart.WinUI.Internal;

[TypeConstraint(typeof(FrameworkElement))]
public sealed class WindowClosingTrigger : Trigger<FrameworkElement>
{
    private AppWindow? window;

    protected override void OnAttached()
    {
        base.OnAttached();

        if (AssociatedObject.IsLoaded)
        {
            Subscribe();
        }
        else
        {
            AssociatedObject.Loaded += OnLoaded;
        }
    }

    protected override void OnDetaching()
    {
        AssociatedObject.Loaded -= OnLoaded;
        Unsubscribe();

        base.OnDetaching();
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        AssociatedObject.Loaded -= OnLoaded;
        Subscribe();
    }

    private void Subscribe()
    {
        window = WindowHelper.GetAppWindow(AssociatedObject);
        if (window is not null)
        {
            window.Closing += OnClosing;
        }
    }

    private void Unsubscribe()
    {
        if (window is not null)
        {
            window.Closing -= OnClosing;
            window = null;
        }
    }

    private void OnClosing(AppWindow sender, AppWindowClosingEventArgs args)
    {
        Interaction.ExecuteActions(AssociatedObject, Actions, args);
    }
}
