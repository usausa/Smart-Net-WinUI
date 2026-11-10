namespace Smart.WinUI.Data;

using Microsoft.UI;
using Microsoft.UI.Xaml.Media;

public sealed class NullToObjectConverterTests
{
    private const string Language = "";

    [Fact]
    public void NullReturnsNullValue()
    {
        // Arrange
        var converter = new NullToTextConverter { NullValue = "null", NonNullValue = "not-null" };

        // Act & Assert
        Assert.Equal("null", converter.Convert(null, typeof(string), null, Language));
    }

    [Fact]
    public void NonNullReturnsNonNullValue()
    {
        // Arrange
        var converter = new NullToTextConverter { NullValue = "null", NonNullValue = "not-null" };

        // Act & Assert
        Assert.Equal("not-null", converter.Convert("something", typeof(string), null, Language));
    }

    [Fact]
    public void HandleEmptyStringTreatsEmptyAsNull()
    {
        // Arrange
        var converter = new NullToTextConverter
        {
            NullValue = "null",
            NonNullValue = "not-null",
            HandleEmptyString = true
        };

        // Act & Assert
        Assert.Equal("null", converter.Convert(string.Empty, typeof(string), null, Language));
    }

    [Fact]
    public void HandleEmptyStringFalseDoesNotTreatEmptyAsNull()
    {
        // Arrange
        var converter = new NullToTextConverter
        {
            NullValue = "null",
            NonNullValue = "not-null",
            HandleEmptyString = false
        };

        // Act & Assert
        Assert.Equal("not-null", converter.Convert(string.Empty, typeof(string), null, Language));
    }

    [Fact]
    public void NullToBoolDefaultValues()
    {
        // Arrange
        var converter = new NullToBoolConverter();

        // Act & Assert
        Assert.Equal(false, converter.Convert(null, typeof(bool), null, Language));
        Assert.Equal(true, converter.Convert("x", typeof(bool), null, Language));
    }

    [Fact]
    public void NullToBrushConverterDefaultsToTransparent()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var converter = new NullToBrushConverter();

            // Act & Assert
            Assert.Equal(Colors.Transparent, Assert.IsType<SolidColorBrush>(converter.Convert(null, typeof(Brush), null, Language)).Color);
            Assert.Equal(Colors.Transparent, Assert.IsType<SolidColorBrush>(converter.Convert("x", typeof(Brush), null, Language)).Color);
        });
    }

    [Fact]
    public void ConvertBackThrows()
    {
        // Arrange
        var converter = new NullToTextConverter();

        // Act & Assert
        Assert.Throws<NotSupportedException>(() => converter.ConvertBack("test", typeof(string), null, Language));
    }
}
