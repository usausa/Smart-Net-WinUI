namespace Smart.Threading;

using Microsoft.UI.Dispatching;

public sealed class ExceptionHandlingSynchronizationContext : DispatcherQueueSynchronizationContext
{
    private readonly DispatcherQueue dispatcherQueue;

    private readonly Func<Exception, bool> handler;

    public ExceptionHandlingSynchronizationContext(DispatcherQueue dispatcherQueue, Func<Exception, bool> handler)
        : base(dispatcherQueue)
    {
        this.dispatcherQueue = dispatcherQueue;
        this.handler = handler;
    }

    public override void Post(SendOrPostCallback d, object? state) =>
        base.Post(s =>
        {
            try
            {
                d(s);
            }
            catch (Exception ex) when (handler(ex))
            {
            }
        }, state);

    public override SynchronizationContext CreateCopy() =>
        new ExceptionHandlingSynchronizationContext(dispatcherQueue, handler);
}
