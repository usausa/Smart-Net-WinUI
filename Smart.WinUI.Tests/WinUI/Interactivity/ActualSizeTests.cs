namespace Smart.WinUI.Interactivity;

using Microsoft.UI.Xaml.Controls;

public sealed class ActualSizeTests
{
    [Fact]
    public void DefaultIsNaN()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var element = new Border();

            // Act
            var width = ActualSize.GetWidth(element);
            var height = ActualSize.GetHeight(element);

            // Assert
            Assert.True(Double.IsNaN(width));
            Assert.True(Double.IsNaN(height));
        });
    }

    [Fact]
    public void EnablingSetsCurrentSize()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var element = new Border();

            // Act
            ActualSize.SetIsEnabled(element, true);

            // Assert
            Assert.Equal(element.ActualWidth, ActualSize.GetWidth(element));
            Assert.Equal(element.ActualHeight, ActualSize.GetHeight(element));
        });
    }
}
