namespace Smart.Threading;

using Microsoft.UI.Dispatching;

public static class DispatcherQueueExtensions
{
    public static Task EnqueueAsync(this DispatcherQueue queue, Action action, CancellationToken cancellationToken = default)
    {
        if (queue.HasThreadAccess && !cancellationToken.IsCancellationRequested)
        {
            return Invoke(action, static x =>
            {
                x();
                return true;
            });
        }

        return queue.EnqueueAsync(ToFunc(action), cancellationToken);
    }

    public static Task EnqueueAsync<T>(this DispatcherQueue queue, Action<T> action, T arg, CancellationToken cancellationToken = default)
    {
        if (queue.HasThreadAccess && !cancellationToken.IsCancellationRequested)
        {
            return Invoke((action, arg), static x =>
            {
                x.action(x.arg);
                return true;
            });
        }

        return queue.EnqueueAsync(ToFunc(action, arg), cancellationToken);
    }

    public static Task<TResult> EnqueueAsync<TResult>(this DispatcherQueue queue, Func<TResult> func, CancellationToken cancellationToken = default)
    {
        if (queue.HasThreadAccess && !cancellationToken.IsCancellationRequested)
        {
            return Invoke(func, static x => x());
        }

        return queue.EnqueueAsync(ToTaskFunc(func), cancellationToken);
    }

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

    private static Func<bool> ToFunc(Action action) => () =>
    {
        action();
        return true;
    };

    private static Func<bool> ToFunc<T>(Action<T> action, T arg) => () =>
    {
        action(arg);
        return true;
    };

    private static Func<Task<TResult>> ToTaskFunc<TResult>(Func<TResult> func) => () => Task.FromResult(func());

#pragma warning disable CA1031
    private static Task<TResult> Invoke<TState, TResult>(TState state, Func<TState, TResult> func)
    {
        try
        {
            return Task.FromResult(func(state));
        }
        catch (Exception ex)
        {
            return Task.FromException<TResult>(ex);
        }
    }

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
