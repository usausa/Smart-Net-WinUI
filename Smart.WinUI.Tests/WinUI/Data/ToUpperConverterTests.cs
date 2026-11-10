namespace Smart.WinUI.Data;

public sealed class ToUpperConverterTests
{
    private const string Language = "";

    [Fact]
    public void ConvertsToUpperCase()
    {
        // Arrange
        var converter = new ToUpperConverter();

        // Act & Assert
        Assert.Equal("HELLO WORLD", converter.Convert("Hello World", typeof(string), null, Language));
    }

    [Fact]
    public void NonStringReturnsNull()
    {
        // Arrange
        var converter = new ToUpperConverter();

        // Act & Assert
        Assert.Null(converter.Convert(42, typeof(string), null, Language));
    }

    [Fact]
    public void ConvertBackThrows()
    {
        // Arrange
        var converter = new ToUpperConverter();

        // Act & Assert
        Assert.Throws<NotSupportedException>(() => converter.ConvertBack("HELLO", typeof(string), null, Language));
    }
}
