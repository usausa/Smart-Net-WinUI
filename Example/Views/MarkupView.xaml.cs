namespace Example.Views;

internal sealed partial class MarkupView
{
    public MarkupView()
    {
        InitializeComponent();
        DataContext = ViewModel;
    }

    public MarkupViewModel ViewModel { get; } = new();
}
