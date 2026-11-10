namespace Example.Views;

using Smart.Mvvm;
using Smart.WinUI.Input;
using Smart.WinUI.ViewModels;

internal sealed partial class MarkupViewModel : ExtendViewModelBase
{
    [ObservableProperty]
    public partial bool Flag { get; set; }

    [ObservableProperty]
    public partial string Text { get; set; }

    [ObservableProperty]
    public partial int Index { get; set; }

    [ObservableProperty]
    public partial string Received { get; set; }

    public DelegateCommand<object?> ShowCommand { get; }

    public MarkupViewModel()
    {
        Text = "Smart";
        Received = "-";
        ShowCommand = new DelegateCommand<object?>(x => Received = x is null ? "(null)" : $"{x} ({x.GetType().Name})");
    }
}
