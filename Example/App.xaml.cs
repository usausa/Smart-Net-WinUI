namespace Example;

using Example.Views;

using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using Smart.Mvvm.Resolver;
using Smart.Threading;

public sealed partial class App
{
    private MainWindow? window;

    private bool showingError;

    public App()
    {
        DefaultResolveProvider.Default.SetResolver(static type => type == typeof(ResolvedViewModel) ? new ResolvedViewModel() : null);
        InitializeComponent();
    }

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        SynchronizationContext.SetSynchronizationContext(new ExceptionHandlingSynchronizationContext(DispatcherQueue.GetForCurrentThread(), HandleException));
        window = new MainWindow();
        window.Activate();
    }

    private bool HandleException(Exception ex)
    {
        if (!showingError && (window?.Content?.XamlRoot is { } root))
        {
            showingError = true;
            var dialog = new ContentDialog { XamlRoot = root, Title = "Error", Content = ex.Message, CloseButtonText = "OK" };
            dialog.Closed += (_, _) => showingError = false;
            _ = dialog.ShowAsync();
        }

        return true;
    }
}
