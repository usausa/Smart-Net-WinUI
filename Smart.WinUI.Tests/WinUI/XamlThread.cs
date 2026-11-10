namespace Smart.WinUI;

using System.Collections.Concurrent;
using System.Diagnostics;

using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml.Hosting;

internal static class XamlThread
{
    private static readonly BlockingCollection<Task> Tasks = new();

    private static readonly Lazy<Thread> Worker = new(Start);

    public static void Run(Action action)
    {
        _ = Worker.Value;
        var task = new Task(action);
        Tasks.Add(task);
        task.GetAwaiter().GetResult();
    }

    public static void ProcessUntil(Func<bool> condition, TimeSpan timeout)
    {
        var queue = DispatcherQueue.GetForCurrentThread();
        var watch = Stopwatch.StartNew();
        var timer = queue.CreateTimer();
        timer.Interval = TimeSpan.FromMilliseconds(1);
        var exiting = false;
        timer.Tick += (_, _) =>
        {
            if (!exiting && (condition() || (watch.Elapsed >= timeout)))
            {
                exiting = true;
                timer.Stop();
                queue.EnqueueEventLoopExit();
            }
        };
        timer.Start();
        queue.RunEventLoop(DispatcherRunOptions.QuitOnlyLocalLoop, new DispatcherExitDeferral());
    }

    private static Thread Start()
    {
        var thread = new Thread(static () =>
        {
            var controller = DispatcherQueueController.CreateOnCurrentThread();
            using var manager = WindowsXamlManager.InitializeForCurrentThread();
            foreach (var task in Tasks.GetConsumingEnumerable())
            {
                task.RunSynchronously();
            }

            controller.ShutdownQueue();
        })
        {
            IsBackground = true
        };
        thread.SetApartmentState(ApartmentState.STA);
        thread.Start();
        return thread;
    }
}
