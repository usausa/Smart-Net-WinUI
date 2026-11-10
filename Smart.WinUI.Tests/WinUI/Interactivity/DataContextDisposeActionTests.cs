namespace Smart.WinUI.Interactivity;

using Microsoft.UI.Xaml.Controls;

public sealed class DataContextDisposeActionTests
{
    private sealed class Disposable : IDisposable
    {
        public bool Disposed { get; private set; }

        public void Dispose() => Disposed = true;
    }

    [Fact]
    public void DisposesDataContext()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var disposable = new Disposable();
            var element = new Border { DataContext = disposable };
            var action = new DataContextDisposeAction();

            // Act
            var result = action.Execute(element, null);

            // Assert
            Assert.True(disposable.Disposed);
            Assert.True((bool)result);
        });
    }

    [Fact]
    public void ReturnsFalseWhenDataContextIsNotDisposable()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var element = new Border { DataContext = "text" };
            var action = new DataContextDisposeAction();

            // Act
            var result = action.Execute(element, null);

            // Assert
            Assert.False((bool)result);
        });
    }
}
