namespace Smart.WinUI.Interactivity;

using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.Xaml.Interactivity;

using Smart.WinUI.Internal;

[TypeConstraint(typeof(FrameworkElement))]
public sealed class MinimizedToHideBehavior : Behavior<FrameworkElement>
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
            window.Changed += OnChanged;
        }
    }

    private void Unsubscribe()
    {
        if (window is not null)
        {
            window.Changed -= OnChanged;
            window = null;
        }
    }

    private static void OnChanged(AppWindow sender, AppWindowChangedEventArgs args)
    {
        if (args.DidPresenterChange && (sender.Presenter is OverlappedPresenter { State: OverlappedPresenterState.Minimized }))
        {
            sender.Hide();
        }
    }
}
