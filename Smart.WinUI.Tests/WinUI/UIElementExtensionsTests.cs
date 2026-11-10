namespace Smart.WinUI;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;

public sealed class UIElementExtensionsTests
{
    [Fact]
    public void RoutedEventAsObservableSubscribesWithHandlerOfEventType()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var button = new Button();
            var observable = button.RoutedEventAsObservable<PointerEventHandler, PointerRoutedEventArgs>(UIElement.PointerPressedEvent, static h => (_, e) => h(e), true);

            // Act
            var exception = Record.Exception(() => observable.Subscribe(static _ => { }).Dispose());

            // Assert
            Assert.Null(exception);
        });
    }

    [Fact]
    public void RoutedEventAsObservableThrowsWhenHandlerTypeDoesNotMatchEvent()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var button = new Button();
            var observable = button.RoutedEventAsObservable<RoutedEventHandler, RoutedEventArgs>(UIElement.PointerPressedEvent, static h => (_, e) => h(e));

            // Act & Assert
            Assert.Throws<InvalidCastException>(() => observable.Subscribe(static _ => { }));
        });
    }
}
