namespace Smart.WinUI.Data;

public sealed class ToLowerConverterTests
{
    private const string Language = "";

    [Fact]
    public void ConvertsToLowerCase()
    {
        // Arrange
        var converter = new ToLowerConverter();

        // Act & Assert
        Assert.Equal("hello world", converter.Convert("Hello World", typeof(string), null, Language));
    }

    [Fact]
    public void NonStringReturnsNull()
    {
        // Arrange
        var converter = new ToLowerConverter();

        // Act & Assert
        Assert.Null(converter.Convert(42, typeof(string), null, Language));
    }

    [Fact]
    public void ConvertBackThrows()
    {
        // Arrange
        var converter = new ToLowerConverter();

        // Act & Assert
        Assert.Throws<NotSupportedException>(() => converter.ConvertBack("hello", typeof(string), null, Language));
    }
}
