namespace Smart.WinUI.Data;

using Microsoft.UI;
using Microsoft.UI.Xaml.Media;

public sealed class ContainsConverterTests
{
    private const string Language = "";

    [Fact]
    public void ContainedValueReturnsTrue()
    {
        // Arrange
        var converter = new ContainsToBoolConverter();
        var list = new List<string> { "a", "b", "c" };

        // Act & Assert
        Assert.Equal(true, converter.Convert("b", typeof(bool), list, Language));
    }

    [Fact]
    public void NotContainedValueReturnsFalse()
    {
        // Arrange
        var converter = new ContainsToBoolConverter();
        var list = new List<string> { "a", "b" };

        // Act & Assert
        Assert.Equal(false, converter.Convert("z", typeof(bool), list, Language));
    }

    [Fact]
    public void NullParameterReturnsFalse()
    {
        // Arrange
        var converter = new ContainsToBoolConverter();

        // Act & Assert
        Assert.Equal(false, converter.Convert("a", typeof(bool), null, Language));
    }

    [Fact]
    public void TextConverterReturnsConfiguredValues()
    {
        // Arrange
        var converter = new ContainsToTextConverter { TrueValue = "in", FalseValue = "out" };
        var list = new List<string> { "a", "b" };

        // Act & Assert
        Assert.Equal("in", converter.Convert("a", typeof(string), list, Language));
        Assert.Equal("out", converter.Convert("z", typeof(string), list, Language));
    }

    [Fact]
    public void BrushConverterDefaultsToTransparent()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var converter = new ContainsToBrushConverter();
            var list = new List<string> { "a" };

            // Act & Assert
            Assert.Equal(Colors.Transparent, Assert.IsType<SolidColorBrush>(converter.Convert("a", typeof(Brush), list, Language)).Color);
            Assert.Equal(Colors.Transparent, Assert.IsType<SolidColorBrush>(converter.Convert("z", typeof(Brush), list, Language)).Color);
        });
    }

    [Fact]
    public void ConvertBackThrows()
    {
        // Arrange
        var converter = new ContainsToBoolConverter();

        // Act & Assert
        Assert.Throws<NotSupportedException>(() => converter.ConvertBack(true, typeof(object), null, Language));
    }
}
