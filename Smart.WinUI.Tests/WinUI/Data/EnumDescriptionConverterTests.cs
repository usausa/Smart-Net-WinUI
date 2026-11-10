namespace Smart.WinUI.Data;

public sealed class EnumDescriptionConverterTests
{
    private const string Language = "";

    private enum TestEnum
    {
        [System.ComponentModel.Description("First Item")]
        First,
        Second
    }

    [Fact]
    public void ReturnsDescriptionWhenPresent()
    {
        // Arrange
        var converter = new EnumDescriptionConverter();

        // Act & Assert
        Assert.Equal("First Item", converter.Convert(TestEnum.First, typeof(string), null, Language));
    }

    [Fact]
    public void ReturnsToStringWhenNoDescription()
    {
        // Arrange
        var converter = new EnumDescriptionConverter();

        // Act & Assert
        Assert.Equal("Second", converter.Convert(TestEnum.Second, typeof(string), null, Language));
    }

    [Fact]
    public void NullReturnsNull()
    {
        // Arrange
        var converter = new EnumDescriptionConverter();

        // Act & Assert
        Assert.Null(converter.Convert(null, typeof(string), null, Language));
    }

    [Fact]
    public void ConvertBackThrows()
    {
        // Arrange
        var converter = new EnumDescriptionConverter();

        // Act & Assert
        Assert.Throws<NotSupportedException>(() => converter.ConvertBack("test", typeof(TestEnum), null, Language));
    }
}
