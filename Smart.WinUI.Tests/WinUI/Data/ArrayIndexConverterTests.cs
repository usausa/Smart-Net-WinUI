namespace Smart.WinUI.Data;

public sealed class ArrayIndexConverterTests
{
    private const string Language = "";

    [Fact]
    public void ConvertReturnsElementAtIndex()
    {
        // Arrange
        var converter = new ArrayIndexConverter();
        var array = new object[] { "a", "b", "c" };

        // Act
        var result = converter.Convert(1, typeof(object), array, Language);

        // Assert
        Assert.Equal("b", result);
    }

    [Fact]
    public void ConvertNullIndexReturnsNull()
    {
        // Arrange
        var converter = new ArrayIndexConverter();

        // Act
        var result = converter.Convert(null, typeof(object), null, Language);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void ConvertNegativeIndexReturnsNull()
    {
        // Arrange
        var converter = new ArrayIndexConverter();
        var array = new object[] { "a", "b", "c" };

        // Act
        var result = converter.Convert(-1, typeof(object), array, Language);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void ConvertIndexOutOfRangeReturnsNull()
    {
        // Arrange
        var converter = new ArrayIndexConverter();
        var array = new object[] { "a", "b", "c" };

        // Act
        var result = converter.Convert(3, typeof(object), array, Language);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void ConvertBackReturnsIndex()
    {
        // Arrange
        var converter = new ArrayIndexConverter();
        var array = new object[] { "x", "y", "z" };

        // Act
        var result = converter.ConvertBack("y", typeof(int), array, Language);

        // Assert
        Assert.Equal(1, result);
    }

    [Fact]
    public void ConvertBackNotFoundReturnsMinusOne()
    {
        // Arrange
        var converter = new ArrayIndexConverter();
        var array = new object[] { "x", "y" };

        // Act
        var result = converter.ConvertBack("z", typeof(int), array, Language);

        // Assert
        Assert.Equal(-1, result);
    }
}
