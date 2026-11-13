namespace Smart.Threading;

using Microsoft.UI.Dispatching;

using Smart.WinUI;

public sealed class ExceptionHandlingSynchronizationContextTests
{
    private static readonly TimeSpan Timeout = TimeSpan.FromSeconds(5);

    [Fact]
    public void PassesExceptionToHandler()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            Exception? handled = null;
            var context = new ExceptionHandlingSynchronizationContext(DispatcherQueue.GetForCurrentThread(), ex =>
            {
                handled = ex;
                return true;
            });

            // Act
            context.Post(static _ => throw new InvalidOperationException("posted"), null);
            XamlThread.ProcessUntil(() => handled is not null, Timeout);

            // Assert
            var exception = Assert.IsType<InvalidOperationException>(handled);
            Assert.Equal("posted", exception.Message);
        });
    }

    [Fact]
    public void ContinuesAfterHandledException()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var called = false;
            var context = new ExceptionHandlingSynchronizationContext(DispatcherQueue.GetForCurrentThread(), static _ => true);

            // Act
            context.Post(static _ => throw new InvalidOperationException(), null);
            context.Post(_ => called = true, null);
            XamlThread.ProcessUntil(() => called, Timeout);

            // Assert
            Assert.True(called);
        });
    }

    [Fact]
    public void CopyPassesExceptionToHandler()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            Exception? handled = null;
            var context = new ExceptionHandlingSynchronizationContext(DispatcherQueue.GetForCurrentThread(), ex =>
            {
                handled = ex;
                return true;
            });

            // Act
            var copy = context.CreateCopy();
            copy.Post(static _ => throw new InvalidOperationException(), null);
            XamlThread.ProcessUntil(() => handled is not null, Timeout);

            // Assert
            Assert.IsType<ExceptionHandlingSynchronizationContext>(copy);
            Assert.IsType<InvalidOperationException>(handled);
        });
    }
}
