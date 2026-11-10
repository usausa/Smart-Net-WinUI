namespace Example.Views;

using System.Globalization;

using Smart.Mvvm;
using Smart.Mvvm.Messaging;
using Smart.WinUI.Input;
using Smart.WinUI.Messaging;
using Smart.WinUI.ViewModels;

internal sealed partial class ExtensionsViewModel : ExtendViewModelBase
{
    private int requested;

    [ObservableProperty]
    public partial int RunCount { get; set; }

    [ObservableProperty]
    public partial int CountValue { get; set; }

    [ObservableProperty]
    public partial int PingCount { get; set; }

    [ObservableProperty]
    public partial int MessageCount { get; set; }

    [ObservableProperty]
    public partial string RequestedValue { get; set; }

    [ObservableProperty]
    public partial string CloseResult { get; set; }

    [ObservableProperty]
    public partial string ResolvedText { get; set; }

    public EventRequest<int> ValueRequest { get; } = new();

    public CancelRequest CloseRequest { get; } = new();

    public ResolveRequest<string?> TextRequest { get; } = new();

    public ValidationFocusRequest FocusRequest { get; } = new();

    public IObserveCommand RunCommand { get; }

    public IObserveCommand CountCommand { get; }

    public IObserveCommand PingCommand { get; }

    public IObserveCommand SendCommand { get; }

    public IObserveCommand ReceiveCommand { get; }

    public IObserveCommand RequestCommand { get; }

    public IObserveCommand RequestedCommand { get; }

    public IObserveCommand CloseCommand { get; }

    public IObserveCommand ResolveCommand { get; }

    public IObserveCommand ValidateCommand { get; }

    public ExtensionsViewModel()
    {
        RequestedValue = string.Empty;
        CloseResult = string.Empty;
        ResolvedText = string.Empty;
        RunCommand = MakeAsyncCommand(RunAsync);
        CountCommand = MakeDelegateCommand(CommandMode.ControlByBusyState, () => CountValue++);
        PingCommand = MakeDelegateCommand(static () => ReactiveMessenger.Default.Send(new PingMessage()));
        SendCommand = MakeDelegateCommand(() => Messenger.Send("Notify"));
        ReceiveCommand = MakeDelegateCommand(CommandMode.Simple, () => MessageCount++);
        RequestCommand = MakeDelegateCommand(() => ValueRequest.Request(++requested));
        RequestedCommand = MakeDelegateCommand<object?>(CommandMode.Simple, x => RequestedValue = Convert.ToString(x, CultureInfo.InvariantCulture) ?? string.Empty);
        CloseCommand = MakeDelegateCommand(() => CloseResult = CloseRequest.IsCancel() ? "canceled" : "closed");
        ResolveCommand = MakeDelegateCommand(() => ResolvedText = TextRequest.Resolve() ?? string.Empty);
        ValidateCommand = MakeDelegateCommand(Validate);
        Subscribe(ReactiveMessenger.Default.Observe<PingMessage>(), _ => PingCount++);
    }

    private async Task RunAsync()
    {
        await Task.Delay(1000).ConfigureAwait(true);
        RunCount++;
    }

    private void Validate()
    {
        Errors.ClearAllErrors();
        Errors.AddError("Second", "Required");
        FocusRequest.FocusRequest();
    }
}
