namespace Smart.WinUI.Resolver;

using Microsoft.UI.Xaml.Controls;

using Smart.Mvvm.Resolver;

[Collection(nameof(ResolveProvider))]
public sealed class DataContextResolverTests : IDisposable
{
    private readonly IServiceProvider original = ResolveProvider.Default.Provider;

    public void Dispose() => ResolveProvider.Default.Provider = original;

    private sealed class StubProvider : IServiceProvider
    {
        private readonly Dictionary<Type, object> services;

        public StubProvider(params object[] services)
        {
            this.services = services.ToDictionary(static x => x.GetType());
        }

        public object? GetService(Type serviceType) => services.GetValueOrDefault(serviceType);
    }

    private sealed class FirstService : IDisposable
    {
        public bool Disposed { get; private set; }

        public void Dispose() => Disposed = true;
    }

    private sealed class SecondService;

    [Fact]
    public void SetsResolvedInstanceToDataContext()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var service = new FirstService();
            ResolveProvider.Default.Provider = new StubProvider(service);
            var element = new Border();

            // Act
            DataContextResolver.SetType(element, typeof(FirstService));

            // Assert
            Assert.Same(service, element.DataContext);
            Assert.Equal(typeof(FirstService), DataContextResolver.GetType(element));
        });
    }

    [Fact]
    public void DisposesPreviousInstanceWhenTypeChanges()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var first = new FirstService();
            var second = new SecondService();
            ResolveProvider.Default.Provider = new StubProvider(first, second);
            var element = new Border();
            DataContextResolver.SetType(element, typeof(FirstService));

            // Act
            DataContextResolver.SetType(element, typeof(SecondService));

            // Assert
            Assert.Same(second, element.DataContext);
            Assert.True(first.Disposed);
        });
    }

    [Fact]
    public void KeepsPreviousInstanceWhenDisposeOnChangedIsFalse()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var first = new FirstService();
            var second = new SecondService();
            ResolveProvider.Default.Provider = new StubProvider(first, second);
            var element = new Border();
            DataContextResolver.SetDisposeOnChanged(element, false);
            DataContextResolver.SetType(element, typeof(FirstService));

            // Act
            DataContextResolver.SetType(element, typeof(SecondService));

            // Assert
            Assert.Same(second, element.DataContext);
            Assert.False(first.Disposed);
        });
    }
}
