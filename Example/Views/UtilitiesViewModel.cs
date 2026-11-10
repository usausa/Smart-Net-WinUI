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

    [ObservableProperty]
    public partial double Hue { get; set; }

    [ObservableProperty]
    public partial string EnqueueResult { get; set; }

    public AsyncCommand EnqueueCommand { get; }

    public UtilitiesViewModel()
    {
        EnqueueResult = string.Empty;
        EnqueueCommand = new AsyncCommand(EnqueueAsync);
    }

    private static void Fail() => throw new InvalidOperationException();

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
