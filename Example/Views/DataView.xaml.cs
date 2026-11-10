namespace Example.Views;

internal sealed partial class DataView
{
    public DataView()
    {
        InitializeComponent();
        DataContext = ViewModel;
    }

    public DataViewModel ViewModel { get; } = new();
}
