namespace Smart.Threading;

using Microsoft.UI.Dispatching;

using Smart.WinUI;

public sealed class DispatcherQueueExtensionsTests
{
    private static readonly TimeSpan Timeout = TimeSpan.FromSeconds(5);

    private static void Throw() => throw new InvalidOperationException();

    [Fact]
    public void RunsInlineOnDispatcherThread()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var queue = DispatcherQueue.GetForCurrentThread();
            var called = false;
            void Call() => called = true;

            // Act
            var task = queue.EnqueueAsync(Call);

            // Assert
            Assert.True(called);
            Assert.True(task.IsCompletedSuccessfully);
        });
    }

    [Fact]
    public void RunsOnDispatcherThreadFromOtherThread()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var queue = DispatcherQueue.GetForCurrentThread();
            var threadId = Environment.CurrentManagedThreadId;
            var calledThreadId = 0;
            void Call() => calledThreadId = Environment.CurrentManagedThreadId;

            // Act
            var task = Task.Run(() => queue.EnqueueAsync(Call));
            XamlThread.ProcessUntil(() => task.IsCompleted, Timeout);

            // Assert
            Assert.True(task.IsCompletedSuccessfully);
            Assert.Equal(threadId, calledThreadId);
        });
    }

    [Fact]
    public void ReturnsResultFromOtherThread()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var queue = DispatcherQueue.GetForCurrentThread();
            var threadId = Environment.CurrentManagedThreadId;

            // Act
            var task = Task.Run(() => queue.EnqueueAsync(static () => Environment.CurrentManagedThreadId));
            XamlThread.ProcessUntil(() => task.IsCompleted, Timeout);

            // Assert
            Assert.Equal(threadId, task.Result);
        });
    }

    [Fact]
    public void PassesArgument()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var queue = DispatcherQueue.GetForCurrentThread();
            string? received = null;

            // Act
            var task = queue.EnqueueAsync(x => received = x, "abc");

            // Assert
            Assert.True(task.IsCompletedSuccessfully);
            Assert.Equal("abc", received);
        });
    }

    [Fact]
    public void WaitsReturnedTask()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var queue = DispatcherQueue.GetForCurrentThread();
            var pending = new TaskCompletionSource<int>();

            // Act
            var task = queue.EnqueueAsync(() => pending.Task);
            var completedBefore = task.IsCompleted;
            pending.SetResult(5);
            XamlThread.ProcessUntil(() => task.IsCompleted, Timeout);

            // Assert
            Assert.False(completedBefore);
            Assert.Equal(5, task.Result);
        });
    }

    [Fact]
    public void WaitsReturnedTaskWithoutResult()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var queue = DispatcherQueue.GetForCurrentThread();
            var pending = new TaskCompletionSource();

            // Act
            var task = queue.EnqueueAsync(() => pending.Task);
            var completedBefore = task.IsCompleted;
            pending.SetResult();
            XamlThread.ProcessUntil(() => task.IsCompleted, Timeout);

            // Assert
            Assert.False(completedBefore);
            Assert.True(task.IsCompletedSuccessfully);
        });
    }

    [Fact]
    public void CapturesException()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var queue = DispatcherQueue.GetForCurrentThread();

            // Act
            var inline = queue.EnqueueAsync(Throw);
            var other = Task.Run(() => queue.EnqueueAsync(Throw));
            XamlThread.ProcessUntil(() => other.IsCompleted, Timeout);

            // Assert
            Assert.IsType<InvalidOperationException>(inline.Exception?.InnerException);
            Assert.IsType<InvalidOperationException>(other.Exception?.InnerException);
        });
    }

    [Fact]
    public void DoesNotRunWhenCanceled()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var queue = DispatcherQueue.GetForCurrentThread();
            var called = false;
            void Call() => called = true;
            using var cts = new CancellationTokenSource();
            cts.Cancel();

            // Act
            var task = queue.EnqueueAsync(Call, cts.Token);

            // Assert
            Assert.True(task.IsCanceled);
            Assert.False(called);
        });
    }

    [Fact]
    public void DoesNotRunWhenCanceledBeforeDispatch()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var queue = DispatcherQueue.GetForCurrentThread();
            var called = false;
            void Call() => called = true;
            using var cts = new CancellationTokenSource();
            var token = cts.Token;
            Task? task = null;

            // Act
            var enqueue = Task.Run(() =>
            {
                task = queue.EnqueueAsync(Call, token);
            }, TestContext.Current.CancellationToken);
            SpinWait.SpinUntil(() => enqueue.IsCompleted, Timeout);
            cts.Cancel();
            XamlThread.ProcessUntil(() => task?.IsCompleted ?? true, Timeout);

            // Assert
            Assert.True(task?.IsCanceled);
            Assert.False(called);
        });
    }

    [Fact]
    public async Task FailsWhenQueueIsShutDown()
    {
        // Arrange
        var controller = DispatcherQueueController.CreateOnDedicatedThread();
        var queue = controller.DispatcherQueue;
        await controller.ShutdownQueueAsync();

        // Act
        var task = queue.EnqueueAsync(static () => { }, TestContext.Current.CancellationToken);

        // Assert
        Assert.IsType<InvalidOperationException>(task.Exception?.InnerException);
    }
}
