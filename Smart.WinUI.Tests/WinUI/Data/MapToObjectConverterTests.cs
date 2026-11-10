namespace Smart.WinUI.Data;

using Microsoft.UI;
using Microsoft.UI.Xaml.Media;

public sealed class MapToObjectConverterTests
{
    private const string Language = "";

    [Fact]
    public void ConvertMatchedEntryReturnsMappedValue()
    {
        // Arrange
        var converter = new MapToTextConverter { DefaultValue = "default" };
        converter.Entries.Add(new MapToTextEntry { Key = 1, Value = "one" });
        converter.Entries.Add(new MapToTextEntry { Key = 2, Value = "two" });

        // Act
        var result = converter.Convert(2, typeof(string), null, Language);

        // Assert
        Assert.Equal("two", result);
    }

    [Fact]
    public void ConvertUnmatchedEntryReturnsDefaultValue()
    {
        // Arrange
        var converter = new MapToTextConverter { DefaultValue = "default" };
        converter.Entries.Add(new MapToTextEntry { Key = 1, Value = "one" });

        // Act
        var result = converter.Convert(9, typeof(string), null, Language);

        // Assert
        Assert.Equal("default", result);
    }

    [Fact]
    public void UnmappedKeyReturnsDefaultValue()
    {
        // Arrange
        var converter = new MapToTextConverter { DefaultValue = "default" };

        // Act & Assert
        Assert.Equal("default", converter.Convert("unknown", typeof(string), null, Language));
    }

    [Fact]
    public void ConvertEnumKeyMatchesSameEnumValue()
    {
        // Arrange
        var converter = new MapToTextConverter { DefaultValue = "default" };
        converter.Entries.Add(new MapToTextEntry { Key = DayOfWeek.Monday, Value = "Mon" });

        // Act & Assert
        Assert.Equal("Mon", converter.Convert(DayOfWeek.Monday, typeof(string), null, Language));
    }

    [Fact]
    public void ConvertDifferentTypeValueReturnsDefaultValue()
    {
        // Arrange
        var converter = new MapToTextConverter { DefaultValue = "default" };
        converter.Entries.Add(new MapToTextEntry { Key = DayOfWeek.Monday, Value = "Mon" });
        converter.Entries.Add(new MapToTextEntry { Key = 1d, Value = "one" });

        // Act & Assert
        Assert.Equal("default", converter.Convert("Monday", typeof(string), null, Language));
        Assert.Equal("default", converter.Convert(1, typeof(string), null, Language));
    }

    [Fact]
    public void NullValueReturnsDefault()
    {
        // Arrange
        var converter = new MapToTextConverter { DefaultValue = "fallback" };

        // Act & Assert
        Assert.Equal("fallback", converter.Convert(null, typeof(string), null, Language));
    }

    [Fact]
    public void BoolConverterReturnsMappedValueOrDefault()
    {
        // Arrange
        var converter = new MapToBoolConverter();
        converter.Entries.Add(new MapToBoolEntry { Key = 1, Value = true });

        // Act & Assert
        Assert.Equal(true, converter.Convert(1, typeof(bool), null, Language));
        Assert.Equal(false, converter.Convert(2, typeof(bool), null, Language));
    }

    [Fact]
    public void BrushConverterReturnsMappedBrushOrTransparent()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var converter = new MapToBrushConverter();
            var red = new SolidColorBrush(Colors.Red);
            converter.Entries.Add(new MapToBrushEntry { Key = 1, Value = red });

            // Act & Assert
            Assert.Same(red, converter.Convert(1, typeof(Brush), null, Language));
            Assert.Equal(Colors.Transparent, Assert.IsType<SolidColorBrush>(converter.Convert(2, typeof(Brush), null, Language)).Color);
        });
    }

    [Fact]
    public void ConvertBackThrows()
    {
        // Arrange
        var converter = new MapToTextConverter();

        // Act & Assert
        Assert.Throws<NotSupportedException>(() => converter.ConvertBack("x", typeof(object), null, Language));
    }
}
