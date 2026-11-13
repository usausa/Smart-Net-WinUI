namespace Example.Views;

using System.Globalization;

using Microsoft.UI.Dispatching;

using Smart.Mvvm;
using Smart.Threading;
using Smart.WinUI.Input;
using Smart.WinUI.ViewModels;

internal sealed partial class UtilitiesViewModel : ExtendViewModelBase
{
    private readonly DispatcherQueue queue = DispatcherQueue.GetForCurrentThread();

    private int linkCount;

    private int keyCount;

    [ObservableProperty]
    public partial double Hue { get; set; }

    [ObservableProperty]
    public partial string EnqueueResult { get; set; }

    [ObservableProperty]
    public partial double BoxWidth { get; set; }

    [ObservableProperty]
    public partial string LinkResult { get; set; }

    [ObservableProperty]
    public partial string KeyText { get; set; }

    public string[] Letters { get; } = ["A", "B", "C"];

    public AsyncCommand EnqueueCommand { get; }

    public DelegateCommand<string?> LinkCommand { get; }

    public DelegateCommand<string?> KeyCommand { get; }

    public AsyncCommand ThrowCommand { get; }

    public UtilitiesViewModel()
    {
        EnqueueResult = string.Empty;
        BoxWidth = 80;
        LinkResult = string.Empty;
        KeyText = string.Empty;
        EnqueueCommand = new AsyncCommand(EnqueueAsync);
        LinkCommand = new DelegateCommand<string?>(x => LinkResult = String.Create(CultureInfo.InvariantCulture, $"{x} {++linkCount}"));
        KeyCommand = new DelegateCommand<string?>(x => KeyText = String.Create(CultureInfo.InvariantCulture, $"{x} {++keyCount}"));
        ThrowCommand = new AsyncCommand(ThrowAsync);
    }

    private static void Fail() => throw new InvalidOperationException();

    private static async Task ThrowAsync()
    {
        await Task.Delay(100).ConfigureAwait(true);
        throw new InvalidOperationException("Sample exception.");
    }

    private async Task EnqueueAsync()
    {
        var threadId = Environment.CurrentManagedThreadId;
        var onDispatcher = await Task.Run(() => queue.EnqueueAsync(() => Environment.CurrentManagedThreadId == threadId)).ConfigureAwait(true);
        string failure;
        try
        {
            await Task.Run(() => queue.EnqueueAsync(Fail)).ConfigureAwait(true);
            failure = "none";
        }
        catch (InvalidOperationException)
        {
            failure = "caught";
        }

        EnqueueResult = String.Create(CultureInfo.InvariantCulture, $"{onDispatcher} {failure}");
    }
}
