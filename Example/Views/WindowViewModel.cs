namespace Example.Views;

using Smart.Mvvm;
using Smart.WinUI.ViewModels;

internal sealed partial class WindowViewModel : ExtendViewModelBase
{
    [ObservableProperty]
    public partial bool CancelClose { get; set; }
}
