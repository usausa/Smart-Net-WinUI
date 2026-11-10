namespace Example;

using Example.Views;

using Microsoft.UI.Xaml;

using Smart.Mvvm.Resolver;

public sealed partial class App
{
    public App()
    {
        DefaultResolveProvider.Default.SetResolver(static type => type == typeof(ResolvedViewModel) ? new ResolvedViewModel() : null);
        InitializeComponent();
    }

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        new MainWindow().Activate();
    }
}
