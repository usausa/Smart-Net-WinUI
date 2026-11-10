namespace Smart.WinUI;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Dispatching;

public sealed class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddWinUIServicesResolvesDispatcherQueueOfResolvingThread()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddWinUIServices();
            var descriptor = Assert.Single(services, static x => x.ServiceType == typeof(DispatcherQueue));

            // Act
            var queue = descriptor.ImplementationFactory!(null!);

            // Assert
            Assert.Same(DispatcherQueue.GetForCurrentThread(), queue);
        });
    }

    [Fact]
    public void AddWinUIServicesThrowsWhenResolvedOutsideUIThread()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddWinUIServices();
        var descriptor = Assert.Single(services, static x => x.ServiceType == typeof(DispatcherQueue));

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => descriptor.ImplementationFactory!(null!));
    }
}
