namespace Example.Views;

internal sealed partial class WindowView
{
    public WindowView()
    {
        InitializeComponent();
    }

    public WindowViewModel ViewModel { get; } = new();
}
