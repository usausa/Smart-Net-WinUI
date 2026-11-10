namespace Smart.WinUI.Data;

using Microsoft.UI;
using Microsoft.UI.Xaml.Media;

public sealed class CompareConverterTests
{
    private const string Language = "";

    [Fact]
    public void EqualReturnsTrue()
    {
        // Arrange
        var converter = new CompareToBoolConverter();

        // Act & Assert
        Assert.Equal(true, converter.Convert(42, typeof(bool), 42, Language));
    }

    [Fact]
    public void NotEqualReturnsFalse()
    {
        // Arrange
        var converter = new CompareToBoolConverter();

        // Act & Assert
        Assert.Equal(false, converter.Convert(42, typeof(bool), 99, Language));
    }

    [Fact]
    public void TextConverterReturnsConfiguredValues()
    {
        // Arrange
        var converter = new CompareToTextConverter { TrueValue = "same", FalseValue = "different" };

        // Act & Assert
        Assert.Equal("same", converter.Convert(42, typeof(string), 42, Language));
        Assert.Equal("different", converter.Convert(42, typeof(string), 99, Language));
    }

    [Fact]
    public void BrushConverterDefaultsToTransparent()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var converter = new CompareToBrushConverter();

            // Act & Assert
            Assert.Equal(Colors.Transparent, Assert.IsType<SolidColorBrush>(converter.Convert(42, typeof(Brush), 42, Language)).Color);
            Assert.Equal(Colors.Transparent, Assert.IsType<SolidColorBrush>(converter.Convert(42, typeof(Brush), 99, Language)).Color);
        });
    }

    [Fact]
    public void ConvertBackThrows()
    {
        // Arrange
        var converter = new CompareToBoolConverter();

        // Act & Assert
        Assert.Throws<NotSupportedException>(() => converter.ConvertBack(true, typeof(object), null, Language));
    }
}
