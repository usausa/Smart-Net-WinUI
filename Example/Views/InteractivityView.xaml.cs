namespace Example.Views;

internal sealed partial class InteractivityView
{
    public InteractivityView()
    {
        InitializeComponent();
    }

    public InteractivityViewModel ViewModel { get; } = new();
}
