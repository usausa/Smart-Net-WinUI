namespace Example.Views;

using System.ComponentModel;

using Smart.Mvvm;
using Smart.WinUI.Input;
using Smart.WinUI.ViewModels;

internal sealed partial class InputViewModel : ExtendViewModelBase
{
    [ObservableProperty]
    public partial bool CanCount { get; set; }

    [ObservableProperty]
    public partial int Count { get; set; }

    [ObservableProperty]
    public partial int Total { get; set; }

    [ObservableProperty]
    public partial bool IsRunning { get; set; }

    [ObservableProperty]
    public partial int RunCount { get; set; }

    [ObservableProperty]
    public partial int WaitTotal { get; set; }

    public DelegateCommand CountCommand { get; }

    public DelegateCommand<int> AddCommand { get; }

    public AsyncCommand RunCommand { get; }

    public AsyncCommand<int> WaitCommand { get; }

    public InputViewModel()
    {
        CountCommand = new DelegateCommand(() => Count++, () => CanCount);
        AddCommand = new DelegateCommand<int>(x => Total += x);
        RunCommand = new AsyncCommand(RunAsync, () => !IsRunning);
        WaitCommand = new AsyncCommand<int>(WaitAsync, _ => !IsRunning);
        CanCount = true;
    }

    protected override void RaisePropertyChanged(PropertyChangedEventArgs args)
    {
        base.RaisePropertyChanged(args);

        if (args.PropertyName == nameof(CanCount))
        {
            CountCommand.RaiseCanExecuteChanged();
        }
        else if (args.PropertyName == nameof(IsRunning))
        {
            RunCommand.RaiseCanExecuteChanged();
            WaitCommand.RaiseCanExecuteChanged();
        }
    }

    private async Task RunAsync()
    {
        IsRunning = true;
        await Task.Delay(1000).ConfigureAwait(true);
        RunCount++;
        IsRunning = false;
    }

    private async Task WaitAsync(int milliseconds)
    {
        IsRunning = true;
        await Task.Delay(milliseconds).ConfigureAwait(true);
        WaitTotal += milliseconds;
        IsRunning = false;
    }
}
