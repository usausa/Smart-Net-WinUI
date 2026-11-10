namespace Smart.WinUI.Data;

using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;

using Windows.UI;

public sealed class ColorToBrushConverterTests
{
    private const string Language = "";

    [Fact]
    public void ConvertColorToSolidColorBrush()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var converter = new ColorToBrushConverter();

            // Act
            var result = converter.Convert(Colors.Red, typeof(SolidColorBrush), null, Language);

            // Assert
            var brush = Assert.IsType<SolidColorBrush>(result);
            Assert.Equal(Colors.Red, brush.Color);
        });
    }

    [Fact]
    public void ConvertNullReturnsNull()
    {
        // Arrange
        var converter = new ColorToBrushConverter();

        // Act & Assert
        Assert.Null(converter.Convert(null, typeof(SolidColorBrush), null, Language));
    }

    [Fact]
    public void ConvertBackBrushToColor()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var converter = new ColorToBrushConverter();
            var brush = new SolidColorBrush(Colors.Green);

            // Act
            var result = converter.ConvertBack(brush, typeof(Color), null, Language);

            // Assert
            Assert.Equal(Colors.Green, result);
        });
    }

    [Fact]
    public void ConvertBackNullReturnsUnsetValue()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var converter = new ColorToBrushConverter();

            // Act & Assert
            Assert.Equal(DependencyProperty.UnsetValue, converter.ConvertBack(null, typeof(Color), null, Language));
        });
    }
}
