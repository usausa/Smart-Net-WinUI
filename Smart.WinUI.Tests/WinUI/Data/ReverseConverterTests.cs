namespace Smart.WinUI.Data;

public sealed class ReverseConverterTests
{
    private const string Language = "";

    [Fact]
    public void TrueReturnsFalse()
    {
        // Arrange
        var converter = new ReverseConverter();

        // Act & Assert
        Assert.Equal(false, converter.Convert(true, typeof(bool), null, Language));
    }

    [Fact]
    public void FalseReturnsTrue()
    {
        // Arrange
        var converter = new ReverseConverter();

        // Act & Assert
        Assert.Equal(true, converter.Convert(false, typeof(bool), null, Language));
    }

    [Fact]
    public void NonBoolPassesThrough()
    {
        // Arrange
        var converter = new ReverseConverter();

        // Act & Assert
        Assert.Equal("hello", converter.Convert("hello", typeof(string), null, Language));
    }

    [Fact]
    public void ConvertBackTrueReturnsFalse()
    {
        // Arrange
        var converter = new ReverseConverter();

        // Act & Assert
        Assert.Equal(false, converter.ConvertBack(true, typeof(bool), null, Language));
    }
}
