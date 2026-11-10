namespace Smart.WinUI.Resolver;

using System.Reflection;

using Smart.Mvvm.Resolver;
using Smart.WinUI.Markup;

[Collection(nameof(ResolveProvider))]
public sealed class ResolveExtensionTests : IDisposable
{
    private readonly IServiceProvider original = ResolveProvider.Default.Provider;

    public void Dispose() => ResolveProvider.Default.Provider = original;

    private sealed class StubProvider : IServiceProvider
    {
        private readonly object? service;

        public StubProvider(object? service)
        {
            this.service = service;
        }

        public object? GetService(Type serviceType) => service;
    }

    private sealed class Service;

    [Fact]
    public void ThrowsWhenTypeIsNotRegistered()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            ResolveProvider.Default.Provider = new StubProvider(null);
            var extension = new ResolveExtension { Type = typeof(Service) };

            // Act & Assert
            var exception = Assert.Throws<TargetInvocationException>(() => MarkupExtensionHelper.Provide(extension));
            var inner = Assert.IsType<InvalidOperationException>(exception.InnerException);
            Assert.Contains(nameof(Service), inner.Message, StringComparison.Ordinal);
        });
    }

    [Fact]
    public void ReturnsResolvedInstance()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var service = new Service();
            ResolveProvider.Default.Provider = new StubProvider(service);
            var extension = new ResolveExtension { Type = typeof(Service) };

            // Act
            var result = MarkupExtensionHelper.Provide(extension);

            // Assert
            Assert.Same(service, result);
        });
    }
}
