namespace Smart.WinUI.Data;

public sealed class ObjectToBoolConverterTests
{
    private const string Language = "";

    [Fact]
    public void MatchingValueReturnsTrue()
    {
        // Arrange
        var converter = new TextToBoolConverter { TrueValue = "yes", FalseValue = "no" };

        // Act & Assert
        Assert.Equal(true, converter.Convert("yes", typeof(bool), null, Language));
    }

    [Fact]
    public void NonMatchingValueReturnsFalse()
    {
        // Arrange
        var converter = new TextToBoolConverter { TrueValue = "yes", FalseValue = "no" };

        // Act & Assert
        Assert.Equal(false, converter.Convert("other", typeof(bool), null, Language));
    }

    [Fact]
    public void ConvertBackTrueReturnsTrueValue()
    {
        // Arrange
        var converter = new TextToBoolConverter { TrueValue = "yes", FalseValue = "no" };

        // Act & Assert
        Assert.Equal("yes", converter.ConvertBack(true, typeof(string), null, Language));
    }

    [Fact]
    public void ConvertBackFalseReturnsFalseValue()
    {
        // Arrange
        var converter = new TextToBoolConverter { TrueValue = "yes", FalseValue = "no" };

        // Act & Assert
        Assert.Equal("no", converter.ConvertBack(false, typeof(string), null, Language));
    }

    [Fact]
    public void IntToBoolConverter()
    {
        // Arrange
        var converter = new IntToBoolConverter { TrueValue = 1, FalseValue = 0 };

        // Act & Assert
        Assert.Equal(true, converter.Convert(1, typeof(bool), null, Language));
        Assert.Equal(false, converter.Convert(0, typeof(bool), null, Language));
        Assert.Equal(false, converter.Convert(99, typeof(bool), null, Language));
    }
}
