namespace Smart.Threading;

using Microsoft.UI.Dispatching;

public static class DispatcherQueueExtensions
{
    public static Task EnqueueAsync(this DispatcherQueue queue, Action action, CancellationToken cancellationToken = default) =>
        queue.EnqueueAsync(() =>
        {
            action();
            return true;
        }, cancellationToken);

    public static Task EnqueueAsync<T>(this DispatcherQueue queue, Action<T> action, T arg, CancellationToken cancellationToken = default) =>
        queue.EnqueueAsync(() =>
        {
            action(arg);
            return true;
        }, cancellationToken);

    public static Task<TResult> EnqueueAsync<TResult>(this DispatcherQueue queue, Func<TResult> func, CancellationToken cancellationToken = default) =>
        queue.EnqueueAsync(() => Task.FromResult(func()), cancellationToken);

    public static Task EnqueueAsync(this DispatcherQueue queue, Func<Task> func, CancellationToken cancellationToken = default) =>
        queue.EnqueueAsync(async () =>
        {
            await func().ConfigureAwait(false);
            return true;
        }, cancellationToken);

    public static Task<TResult> EnqueueAsync<TResult>(this DispatcherQueue queue, Func<Task<TResult>> func, CancellationToken cancellationToken = default)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return Task.FromCanceled<TResult>(cancellationToken);
        }

        var source = new TaskCompletionSource<TResult>(TaskCreationOptions.RunContinuationsAsynchronously);
        if (queue.HasThreadAccess)
        {
            _ = CompleteAsync(source, func);
        }
        else if (!queue.TryEnqueue(() =>
                 {
                     if (cancellationToken.IsCancellationRequested)
                     {
                         source.SetCanceled(cancellationToken);
                     }
                     else
                     {
                         _ = CompleteAsync(source, func);
                     }
                 }))
        {
            source.SetException(new InvalidOperationException("Failed to enqueue."));
        }

        return source.Task;
    }

#pragma warning disable CA1031
    private static async Task CompleteAsync<TResult>(TaskCompletionSource<TResult> source, Func<Task<TResult>> func)
    {
        try
        {
            source.SetResult(await func().ConfigureAwait(false));
        }
        catch (Exception ex)
        {
            source.SetException(ex);
        }
    }
#pragma warning restore CA1031
}
