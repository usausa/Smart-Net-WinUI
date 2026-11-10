namespace Example.Views;

internal sealed partial class InputView
{
    public InputView()
    {
        InitializeComponent();
    }

    public InputViewModel ViewModel { get; } = new();
}
