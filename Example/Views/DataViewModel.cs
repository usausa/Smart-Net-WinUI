namespace Example.Views;

using Smart.Mvvm;
using Smart.WinUI.ViewModels;

[WinRT.GeneratedBindableCustomProperty]
internal sealed partial class DataViewModel : ExtendViewModelBase
{
    [ObservableProperty]
    public partial bool Flag { get; set; }

    [ObservableProperty]
    public partial string Text { get; set; }

    [ObservableProperty]
    public partial double Number { get; set; }

    [ObservableProperty]
    public partial string Size { get; set; }

    [ObservableProperty]
    public partial SampleKind Kind { get; set; }

    [ObservableProperty]
    public partial string Amount { get; set; }

    [ObservableProperty]
    public partial int Index { get; set; }

    public DataViewModel()
    {
        Text = "Smart";
        Number = 50;
        Size = "Medium";
        Kind = SampleKind.Apple;
        Amount = "30";
    }
}
